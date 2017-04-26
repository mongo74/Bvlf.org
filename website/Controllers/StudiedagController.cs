using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using bvlf_v2.BOL;
using bvlf_v2.BOL.Data;
using bvlf_v2.BOL.Helpers;
using bvlf_v2.BOL.Repositories;
using bvlf_v2.Helpers;
using bvlf_v2.Models;
using umbraco.BusinessLogic;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;

namespace bvlf_v2.Controllers
{
    public class StudiedagController : SurfaceController
    {
        private readonly IContentService _contentService;
        private readonly EmailSender _emailSender;
        private readonly MembershipRepository _repository;
        private readonly SubscriptionRepository _subscriptionRepo;
        public IEnumerable<IContent> sessions;

        public StudiedagController()
        {
            _emailSender = new EmailSender(Settings.SmtpServer);
            _repository = new MembershipRepository();
            _subscriptionRepo = new SubscriptionRepository();


            _contentService = Services.ContentService;
            var _contentTypeService = Services.ContentTypeService;
            var contentType = _contentTypeService.GetContentType("StudiedagAtelier");
            sessions = _contentService.GetContentOfContentType(contentType.Id).Where(p => p.Published);

            XmlSessions = GetXmlSessions(sessions);
        }

        public Dictionary<int, XmlDocument> XmlSessions { get; set; }

        private Dictionary<int, XmlDocument> GetXmlSessions(IEnumerable<IContent> sessions)
        {
            var returnList = new Dictionary<int, XmlDocument>();
            var sessionIds = sessions.Select(session => session.Id).ToList();
            returnList = _subscriptionRepo.GetSessionsAsXml(sessionIds);
            return returnList;
        }

        [OutputCache(Duration = 0)]
        public JsonResult GetSessionsForCalendar()
        {
            try
            {
                var xmlsessions = XmlSessions;
                var returnlist = xmlsessions.Select(p => GetSessionForCalendar(p.Value.InnerXml)).ToList();
                return Json(returnlist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Success = false, message = ex.ToString()}, JsonRequestBehavior.AllowGet);
            }
        }

        private static string ParseDateToTime(string nodedate)
        {
            var date = DateTime.Parse(nodedate);
            return date.ToString("HH:mm");
        }

        private static string ParseDate(string nodedate)
        {
            var currentculture = new CultureInfo("nl-BE");

            var date = DateTime.Parse(nodedate);
            return date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK", currentculture);
        }

        public SessionForCalendar GetSessionForCalendar(string session)
        {
            var xml = new XmlDocument();
            xml.LoadXml(session);

            return new SessionForCalendar
            {
                id = xml.GetElementsByTagName("StudiedagAtelier").Item(0).Attributes["id"].InnerText,
                end = xml.GetElementsByTagName("at_EndTime").Item(0).InnerText,
                start = xml.GetElementsByTagName("at_StartTime").Item(0).InnerText,
                title = string.Format("at {0}", xml.GetElementsByTagName("at_Nummer").Item(0).InnerText)
            };
        }

        public ActionResult GetSessionDetails(int id)
        {
            var _contentService = Services.ContentService;
            var contentitem = _contentService.GetById(id);

            var session = new DetailSessionForJson
            {
                at_Description = contentitem.GetValue("at_Description").ToString(),
                at_Doelgroep = contentitem.GetValue("at_Doelgroep").ToString(),
                at_Maxplaces = Convert.ToInt32(contentitem.GetValue("at_Maxplaces")),
                at_Metdesteunvan = contentitem.GetValue("at_Metdesteunvan").ToString(),
                at_Nummer = contentitem.GetValue("at_Nummer").ToString(),
                at_Type = contentitem.GetValue("at_Type").ToString(),
                at_Speaker = contentitem.GetValue("at_Speaker").ToString(),
                at_SpeakerInfo = contentitem.GetValue("at_SpeakerInfo").ToString(),
                at_EndTime = ParseDateToTime(contentitem.GetValue("at_EndTime").ToString()),
                at_StartTime = ParseDateToTime(contentitem.GetValue("at_StartTime").ToString()),
                title = contentitem.GetValue("at_Title").ToString(),
                at_Iseenherhaling = contentitem.GetValue("at_Iseenherhaling").ToString() != "0",
                id = id.ToString(),
                isVolzet = contentitem.GetValue("isVolzet").ToString().ToLower() == "true"
            };
            return Content(ParseSessionDetailString(session));
        }

        private static string ParseSessionDetailString(DetailSessionForJson session)
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("<h5>Atelier {0} - {1}</h5>", session.at_Nummer, session.title));
            sb.AppendLine(string.Format("<p class=\"LectorDescription\">"));
            sb.AppendLine(string.Format("<strong>{0}</strong><br />", session.at_Speaker));
            sb.AppendLine(string.Format("<span>{0}</span>", session.at_SpeakerInfo));
            sb.AppendLine(string.Format("</p>"));
            if (!string.IsNullOrEmpty(session.at_Metdesteunvan))
            {
                sb.AppendLine(string.Format("<p><strong>{0}</strong></p>", session.at_Metdesteunvan));
            }
            sb.AppendLine(string.Format("<p>{0}</p>", session.at_Description));
            sb.AppendLine(string.Format("<p><strong>{0}</strong></p>", session.at_Doelgroep));
            sb.AppendLine(string.Format("<p>Max <strong>{0}</strong> plaatsen - {1}h-{2}h </p>", session.at_Maxplaces,
                session.at_StartTime, session.at_EndTime));

            return sb.ToString();
        }

        public ActionResult SubscribeToStudieDag(int id)
        {
            if (id != -1)
            {
                var model = MemberProfileViewModel.GetMemberProfileViewModel(id);
                return PartialView("SubscribeStudiedagStep1", model);
            }
            else
            {
                var model = MemberProfileViewModel.GetMemberProfileViewModel();
                return PartialView("SubscribeStudiedagStep1", model);
            }
        }

        [HttpPost]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult HandleSubscribeTostudiedagStep1(MemberProfileViewModel model)
        {
            var result = string.Empty;

            if (model.Id != -1)
            {
                result = _repository.UpdateMemberByAdmin(model);
            }
            else
            {
                model.Id = _repository.CreateNewMemberForStudiedagOnly(model);
                result = "ok";
            }

            if (result == "ok")
            {
                var newmodel = new SubscribeToStudieDagStep2ViewModel();
                var currentSubsciptionCount = _subscriptionRepo.GetCurrentSubscriptionsCount();

                var sessionsForThisSubscriber = new List<int>();
                var existingSubscription = _subscriptionRepo.GetSubscriptionBySubscriberId(model.Id);
                if (existingSubscription != null)
                {
                    var subscribedSessions = existingSubscription.StudieDagSubscriptions_Sessions;
                    sessionsForThisSubscriber.AddRange(subscribedSessions.Select(session => session.SessionId));
                    newmodel.isUpdate = true;
                }

                BuildModel(model, currentSubsciptionCount, sessionsForThisSubscriber, newmodel);

                return Json(new {error = false, form = RenderRazorViewToString("SubscribeStudiedagStep2", newmodel)});
            }
            ModelState.AddModelError(string.Empty, result);
            return Json(new {error = true, form = RenderRazorViewToString("SubscribeStudiedagStep1", model)});
        }

        private void BuildModel(MemberProfileViewModel model, Dictionary<int, int> currentSubsciptionCount,
            List<int> sessionsForThisSubscriber,
            SubscribeToStudieDagStep2ViewModel newmodel)
        {
            var sessionsList = sessions.Select(p => new SessionForJson
            {
                id = p.Id.ToString(),
                isVolzet = CheckSessionVolzet(p, currentSubsciptionCount),
                Group = p.GetValue("group").ToString(),
                at_Nummer = p.GetValue("at_Nummer").ToString(),
                title = p.GetValue("at_Title").ToString(),
                Selected = sessionsForThisSubscriber.Contains(p.Id)
            }).ToList();

            newmodel.cbSessionGroup1List = BuildSessionGroup(sessionsList, "1");
            newmodel.cbSessionGroup2List = BuildSessionGroup(sessionsList, "2");
            newmodel.cbSessionGroup3List = BuildSessionGroup(sessionsList, "3");
            newmodel.cbSessionGroup4List = BuildSessionGroup(sessionsList, "4");
            newmodel.cbSessionGroup5List = BuildSessionGroup(sessionsList, "5");
            newmodel.cbSessionGroup6List = BuildSessionGroup(sessionsList, "6");
            newmodel.cbSessionGroup7List = BuildSessionGroup(sessionsList, "7");
            newmodel.cbSessionGroup8List = BuildSessionGroup(sessionsList, "8");
            //newmodel.cbSessionGroup9List = BuildSessionGroup(sessionsList, "9");
            //newmodel.cbSessionGroup10List = BuildSessionGroup(sessionsList, "10");
            //newmodel.cbSessionGroup11List = BuildSessionGroup(sessionsList, "11");
            //newmodel.cbSessionGroup12List = BuildSessionGroup(sessionsList, "12");
            //newmodel.cbSessionGroup13List = BuildSessionGroup(sessionsList, "13");

            newmodel.SelectedGroup1 = GetSelectedSession(newmodel.cbSessionGroup1List);
            newmodel.SelectedGroup2 = GetSelectedSession(newmodel.cbSessionGroup2List);
            newmodel.SelectedGroup3 = GetSelectedSession(newmodel.cbSessionGroup3List);
            newmodel.SelectedGroup4 = GetSelectedSession(newmodel.cbSessionGroup4List);
            newmodel.SelectedGroup5 = GetSelectedSession(newmodel.cbSessionGroup5List);
            newmodel.SelectedGroup6 = GetSelectedSession(newmodel.cbSessionGroup6List);
            newmodel.SelectedGroup7 = GetSelectedSession(newmodel.cbSessionGroup7List);
            newmodel.SelectedGroup8 = GetSelectedSession(newmodel.cbSessionGroup8List);
            //newmodel.SelectedGroup9 = GetSelectedSession(newmodel.cbSessionGroup9List);
            //newmodel.SelectedGroup10 = GetSelectedSession(newmodel.cbSessionGroup10List);
            //newmodel.SelectedGroup11 = GetSelectedSession(newmodel.cbSessionGroup11List);
            //newmodel.SelectedGroup12 = GetSelectedSession(newmodel.cbSessionGroup12List);
            //newmodel.SelectedGroup13 = GetSelectedSession(newmodel.cbSessionGroup13List);
            newmodel.SubscriberId = model.Id;
        }

        [HttpPost]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult HandleSubscribeTostudiedagStep1Skip(MemberProfileViewModel model)
        {
            var newmodel = new SubscribeToStudieDagStep2ViewModel();
            var currentSubsciptionCount = _subscriptionRepo.GetCurrentSubscriptionsCount();

            var sessionsForThisSubscriber = new List<int>();
            var existingSubscription = _subscriptionRepo.GetSubscriptionBySubscriberId(model.Id);
            if (existingSubscription != null)
            {
                var subscribedSessions = existingSubscription.StudieDagSubscriptions_Sessions;
                sessionsForThisSubscriber.AddRange(subscribedSessions.Select(session => session.SessionId));
                newmodel.isUpdate = true;
            }

            BuildModel(model, currentSubsciptionCount, sessionsForThisSubscriber, newmodel);

            return Json(new {error = false, form = RenderRazorViewToString("SubscribeStudiedagStep2", newmodel)});
        }

        [HttpPost]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult HandleSubscriptionStudieDagStep2(SubscribeToStudieDagStep2ViewModel model)
        {
            try
            {
                ValidateStep2(model);
                if (ModelState.IsValid)
                {
                    var sub = new StudieDagSubscription();
                    sub.SubscriberId = model.SubscriberId;
                    sub.Year = Settings.StudiedagYear;
                    sub.PaidBySchool = model.PaidBySchool;
                    sub.SubscriptionStatus = (int) SubscriptionStatus.Ingeschreven;
                    sub.SubscriptionDate = DateTime.Now;

                    var sessions = new List<StudieDagSubscriptions_Sessions>();

                    AddSession(model.SelectedGroup1, sessions);
                    AddSession(model.SelectedGroup2, sessions);
                    AddSession(model.SelectedGroup3, sessions);
                    AddSession(model.SelectedGroup4, sessions);
                    AddSession(model.SelectedGroup5, sessions);
                    AddSession(model.SelectedGroup6, sessions);
                    AddSession(model.SelectedGroup7, sessions);
                    AddSession(model.SelectedGroup8, sessions);
                    //AddSession(model.SelectedGroup9, sessions);
                    //AddSession(model.SelectedGroup10, sessions);
                    //AddSession(model.SelectedGroup11, sessions);
                    //AddSession(model.SelectedGroup12, sessions);
                    sub.StudieDagSubscriptions_Sessions = sessions;

                    if (model.isUpdate)
                    {
                        _subscriptionRepo.UpdateSubscription(sub);
                    }
                    else
                    {
                        _subscriptionRepo.SaveSubscription(sub);
                        // Send confirmation Mail
                        //  SendConfirmationMail(sub);
                    }
                    SendConfirmationMail(sub);
                    var newModel = new SubscribeToStudieDagStep3ViewModel();

                    return Json(new {error = true, form = RenderRazorViewToString("SubscribeStudiedagStep3", newModel)});
                }
                return Json(new {error = true, form = RenderRazorViewToString("SubscribeStudiedagStep2", model)});
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, string.Format("{0} <br />{1}", ex.Message, ex.StackTrace));
                Log.Add(LogTypes.Error, -1, string.Format("{0} <br />{1}", ex.Message, ex.StackTrace));
                return Json(new {error = true, form = RenderRazorViewToString("SubscribeStudiedagStep2", model)});
            }
        }

        private void SendConfirmationMail(StudieDagSubscription sub)
        {
            var _templateRenderer = new TemplateRenderer(HttpContext.Server.MapPath("~/Templates"));
            var member = _repository.GetMemberById(sub.SubscriberId);

            var mailto = member.Email;
            var mailfrom = ConfigurationManager.AppSettings["Mailfrom"];
            var body = _templateRenderer.RenderTemplate("ConfirmationStudiedag.txt", new
            {
                Voornaam = ViewModelHelper.GetStringValue(member.getProperty("voornaam")),
                Naam = ViewModelHelper.GetStringValue(member.getProperty("naam")),
                Straat = ViewModelHelper.GetStringValue(member.getProperty("straat")),
                nr = ViewModelHelper.GetStringValue(member.getProperty("nr")),
                Bus = ViewModelHelper.GetStringValue(member.getProperty("bus")),
                Zipcode = ViewModelHelper.GetStringValue(member.getProperty("postcode")),
                Location = ViewModelHelper.GetStringValue(member.getProperty("plaats")),
                Email = ViewModelHelper.GetStringValue(member.getProperty("email")),
                School = ViewModelHelper.GetStringValue(member.getProperty("naamschool_1")),
                SchoolStraat = ViewModelHelper.GetStringValue(member.getProperty("straatschool_1")),
                Schoolnr = ViewModelHelper.GetStringValue(member.getProperty("nrSchool_1")),
                SchoolBus = ViewModelHelper.GetStringValue(member.getProperty("busschool_1")),
                SchoolZipcode = ViewModelHelper.GetStringValue(member.getProperty("zipcodeschool_1")),
                SchoolLocation = ViewModelHelper.GetStringValue(member.getProperty("plaatsschool_1")),
                SchoolEmail = ViewModelHelper.GetStringValue(member.getProperty("emailschool_1")),
                SchoolTelefoon = ViewModelHelper.GetStringValue(member.getProperty("telschool_1")),
                SessieList = ParseSessionlist(sub.StudieDagSubscriptions_Sessions),
                Telefoon = ViewModelHelper.GetStringValue(member.getProperty("tel")),
                GSM = ViewModelHelper.GetStringValue(member.getProperty("mobile"))
            });
            var subject = "Bevestiging van uw inschrijving";
            var emailSender = new EmailSender(Settings.SmtpServer);
            emailSender.SendEmail(mailfrom, "BVLF vzw", new List<string> {mailto}, new List<string>(), subject, body,
                null, false);
        }

        private void SendPaimentConfirmationMail(StudieDagSubscription sub)
        {
            var _templateRenderer = new TemplateRenderer(HttpContext.Server.MapPath("~/Templates"));
            var member = _repository.GetMemberById(sub.SubscriberId);
            var mailto = member.Email;
            var mailfrom = ConfigurationManager.AppSettings["Mailfrom"];
            var body = _templateRenderer.RenderTemplate("ConfirmationPaymentStudiedag.txt", new {});

            var subject = string.Format("BVLF Studiedag {0} - Bevestiging van uw betaling",
                ConfigurationManager.AppSettings["StudiedagYear"]);
            _emailSender.SendEmail(mailfrom, "", new List<string> {mailto}, new List<string>(), subject, body, null,
                false);
        }

        private string ParseSessionlist(IEnumerable<StudieDagSubscriptions_Sessions> studieDagSubscriptionsSessions)
        {
            var sb = new StringBuilder();
            foreach (var s in studieDagSubscriptionsSessions)
            {
                var session = sessions.FirstOrDefault(p => p.Id == s.SessionId);
                sb.AppendLine(string.Format("Atelier {0} - {1}", session.GetValue("at_Nummer"),
                    session.GetValue("at_Title")));
            }
            return sb.ToString();
        }

        private void ValidateStep2(SubscribeToStudieDagStep2ViewModel model)
        {
            if (!model.HasReadConditions)
            {
                ModelState.AddModelError(string.Empty,
                    "U dient aan te duiden of u de inschrijvingsvoorwaarden hebt gelezen en ermee akkoord bent");
            }
        }

        private string GetSelectedSession(CheckBoxListViewModel<string> cbSessionGroup1List)
        {
            var item = cbSessionGroup1List.ListItems.FirstOrDefault(p => p.Selected);
            return item != null ? item.Value : "0";
        }

        private CheckBoxListViewModel<string> BuildSessionGroup(IEnumerable<SessionForJson> sessionsList,
            string groupname)
        {
            var selectedItems = sessionsList.Where(p => p.Group == groupname).OrderBy(p => p.id);

            var returnlist = new CheckBoxListViewModel<string> {ListItems = new List<CheckBoxListItem<string>>()};
            foreach (var session in selectedItems)
            {
                returnlist.ListItems.Add(new CheckBoxListItem<string>
                {
                    ClassName = session.Group,
                    IsVolzet = session.isVolzet,
                    Selected = session.Selected,
                    Text = session.at_Nummer,
                    Value = session.id
                });
            }
            return returnlist;
        }

        private bool CheckSessionVolzet(IContent content, Dictionary<int, int> subscriptionCount)
        {
            var item = subscriptionCount.FirstOrDefault(k => k.Key == content.Id);
            var maxPlaces = Convert.ToInt32(content.GetValue("at_Maxplaces").ToString());
            if (content.GetValue("isVolzet").ToString() == "1")
            {
                return true;
            }
            if (item.Value < maxPlaces) return false;
            ZetAtelierVolzet(content);
            return true;
        }

        private void ZetAtelierVolzet(IContent content)
        {
            content.SetValue("isVolzet", true);
            _contentService.SaveAndPublish(content);
        }

        public ActionResult SubscribeStudiedagStep3()
        {
            var newmodel = new SubscribeToStudieDagStep3ViewModel();
            return PartialView("SubscribeStudiedagStep3", newmodel);
        }

        public ActionResult SubscribeTostudiedagStep2()
        {
            var contentTypeService = Services.ContentTypeService;
            var contenttyepe = contentTypeService.GetContentType("StudiedagAtelier");

            var studiedagSessions = _contentService.GetContentOfContentType(contenttyepe.Id).Where(p => p.Published);

            var sessionsList = studiedagSessions.Select(p => new SessionForJson
            {
                id = p.Id.ToString(CultureInfo.InvariantCulture),
                isVolzet = false,
                Group = p.GetValue("group").ToString(),
                at_Nummer = p.GetValue("at_Nummer").ToString(),
                title = p.GetValue("at_Title").ToString()
            }).ToList();
            var newmodel = new SubscribeToStudieDagStep2ViewModel
            {
                cbSessionGroup1List = BuildSessionGroup(sessionsList, "1"),
                cbSessionGroup2List = BuildSessionGroup(sessionsList, "2"),
                cbSessionGroup3List = BuildSessionGroup(sessionsList, "3"),
                cbSessionGroup4List = BuildSessionGroup(sessionsList, "4"),
                cbSessionGroup5List = BuildSessionGroup(sessionsList, "5"),
                cbSessionGroup6List = BuildSessionGroup(sessionsList, "6"),
                cbSessionGroup7List = BuildSessionGroup(sessionsList, "7"),
                cbSessionGroup8List = BuildSessionGroup(sessionsList, "8")
                //cbSessionGroup9List = BuildSessionGroup(SessionsList, "9"),
                //cbSessionGroup10List = BuildSessionGroup(SessionsList, "10")
            };
            return PartialView("SubscribeStudiedagStep2", newmodel);
        }

        private static void AddSession(string modelSessionvalue, List<StudieDagSubscriptions_Sessions> sessions)
        {
            if (!string.IsNullOrEmpty(modelSessionvalue) && modelSessionvalue != "0")
            {
                sessions.Add(new StudieDagSubscriptions_Sessions
                {
                    SessionId = Convert.ToInt32(modelSessionvalue)
                });
            }
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public ActionResult LoadInschrijvingen()
        {
            var model = new StudiedagInschrijvingenSearchModel();
            return PartialView("InschrijvingenSearch", model);
        }

        [OutputCache(Duration = 60, VaryByParam = "*")]
        public ActionResult GetAllInschrijvingen()
        {
            var subscriptions = _subscriptionRepo.GetAllSubscriptions();

            var model = new StudiedagInschrijvingenLijstViewModel
            {
                Inschrijvingen = ViewModelHelper.GetInschrijvingenLijstViewModelByEF(subscriptions, XmlSessions)
            };
            return PartialView("AjaxViews/InschrijvingenList", model);
        }

        [OutputCache(Duration = 0, VaryByParam = "*")]
        public JsonResult ConfirmStudiedagPayment(int subscriptionid)
        {
            try
            {
                _subscriptionRepo.SetSubscriberBetaald(subscriptionid);
                var subscription = _subscriptionRepo.GetSubscriptionBySubscriptionId(subscriptionid);
                SendPaimentConfirmationMail(subscription);
                return Json(new {Success = true}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Success = false}, JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(Duration = 0, VaryByParam = "*")]
        public JsonResult CancelSubscription(int subscriptionid)
        {
            try
            {
                _subscriptionRepo.CancelSubscription(subscriptionid);
                return Json(new {Success = true}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Success = false}, JsonRequestBehavior.AllowGet);
            }
        }

        #region Excell Export

        public FileContentResult ExportInschrijvingenList()
        {
            var filename = string.Format("StudiedagInschrijvingen_{0}.xlsx", DateTime.Now.Ticks);
            var model = GetInschrijvingenForExportViewModel();
            //var model = GetAllInschrijvingenViewModel();
            var outputstream = ExcellWriterHelper.CreateStudiedagInschrijvingenExportFile(model);
            //CreateExcelAsByteArray(data);
            return File(outputstream, "application/vnd.ms-excel", filename);
        }

        #endregion

        private StudiedagInschrijvingforExportViewModel GetInschrijvingenForExportViewModel()
        {
            var subscriptions = _subscriptionRepo.GetAllSubscriptions();
            var model = new StudiedagInschrijvingforExportViewModel
            {
                Inschrijvingen =
                    ViewModelHelper.GetInschrijvingenLijstForExportViewModelByEF(subscriptions, XmlSessions)
            };
            return model;
        }
    }

    public class SessionForCalendar
    {
        public string id { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string title { get; set; }
    }
}