using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using bvlf_v2.BOL;
using bvlf_v2.BOL.Helpers;
using bvlf_v2.BOL.Repositories;
using bvlf_v2.Helpers;
using bvlf_v2.Models;
using umbraco.cms.businesslogic.member;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;

namespace bvlf_v2.Controllers
{
    /// <summary>
    ///     Controller for AJAX membership Calls
    ///     Not in Production yet
    /// </summary>
    public class MemberShipApiController : SurfaceController
    {
        private readonly MembershipRepository _repository;

        public MemberShipApiController()
        {
            _repository = new MembershipRepository();
        }

        [Umbraco.Web.WebApi.MemberAuthorize(AllowType = "Retailers")]
        //[OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult GetAllMembers()
        {
            var memberlist = _repository.GetAllMembers();

            var list = ViewModelHelper.GetMemberProfilesListViewModel(memberlist).ToList();

            var filteredlist = list.Where(p => p != null);

            var memberStatus = BvlfMemberStatus.Studiedag.ToString();
            var returnlist = filteredlist.Where(p => p.Status.ToLower() != memberStatus.ToLower()).ToList();
            var model = new MemberProfilesListViewModel
            {
                MemberList = returnlist
            };

            return PartialView("AjaxViews/MemberList", model);
        }

        /// <summary>
        ///     Calls all members (non-filtered) and returns a Json string
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult GetAllMembersJson()
        {
            var list = _repository.GetAllMembers();
            var returnlist = ViewModelHelper.GetMemberProfilesListViewModel(list);
            return Json(returnlist, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetMembersBySearchCriteria(string searchString, int MemberStatus, string cb)
        {
            return SearchResults(searchString, cb, MemberStatus);
        }

        /// <summary>
        ///     Filtered search results
        /// </summary>
        /// <param name="searchString">string filter</param>
        /// <param name="cb">filter checkboxstatus</param>
        /// <param name="memberStatus">Filter memberstatus</param>
        /// <returns></returns>
        private JsonResult SearchResults(string searchString, string cb, int memberStatus)
        {
            var list = _repository.GetAllMembers();

            var returnList = ViewModelHelper.GetMemberProfilesListViewModel(list).ToList();
            switch (cb.ToLower())
            {
                case "email":
                    returnList =
                        returnList.Where(
                            p => p.Email.IndexOf(searchString, StringComparison.InvariantCultureIgnoreCase) > 0)
                            .ToList();
                    break;
                case "lidnr":
                    returnList =
                        returnList.Where(
                            p => p.Lidnr.IndexOf(searchString, StringComparison.InvariantCultureIgnoreCase) > 0)
                            .ToList();
                    break;
                case "name":
                    returnList =
                        returnList.Where(
                            p => p.FullName.IndexOf(searchString, StringComparison.InvariantCultureIgnoreCase) >= 0)
                            .ToList();
                    break;
            }

            if (memberStatus == 0)
            {
                return Json(returnList, JsonRequestBehavior.AllowGet);
            }
            var endlist =
                returnList.Where(
                    p =>
                        String.Equals(p.Status, ((BvlfMemberStatus) memberStatus).ToString(),
                            StringComparison.CurrentCultureIgnoreCase));

            return Json(endlist, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Gets member information by MemberId
        /// </summary>
        /// <param name="id">Identification number of member</param>
        /// <returns></returns>
        public ActionResult GetMemberDetails(int id)
        {
            var model = MemberProfileViewModel.GetMemberProfileViewModel(id);
            return PartialView("AjaxViews/MemberDetails", model);
        }

        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult GetMemberById(int id)
        {
            try
            {
                var model = _repository.GetCmsMemberById(id);
                var returnModel = ViewModelHelper.GetMemberProfileForListViewModel(model);
                return Json(returnModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Success = false}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [OutputCache(Duration = 0, VaryByParam = "*")]
        public ActionResult UpdateProfile(MemberProfileForAdminUpdateViewModel model)
        {
            if (!ModelState.IsValid) return Json(new {msg = "Form not valid"});
            var result = _repository.UpdateMemberByAdmin(model);
            return result == "ok"
                ? Json(new
                {
                    msg = "success",
                    userid = model.Id,
                    FullName = string.Format("{0} {1}",
                        model.FirstName, model.LastName),
                    school = model.School1.SchoolNaam
                })
                : Json(new {msg = result});
        }

        public ActionResult SendReminderMails()
        {
            var model = new RemindermailsModel();
            var memberlist = _repository.GetAllMembers().OrderBy(p => p.nodeId).ToList();
            var willExpireSoonList =
                ViewModelHelper.GetMemberProfilesListViewModel(memberlist)
                    .Where(p => !p.IsRemoved && p.MembershipWilExpireSoon)
                    .Where(p => !p.HasReceivedReminderMail)
                    .Take(25)
                    .ToList();
            //var testList = ViewModelHelper.GetMemberProfilesListViewModel(memberlist)
            //        .Where(p => !p.IsRemoved && p.MembershipWilExpireSoon).Where(p => p.HasReceivedReminderMail)
            //   .ToList();

            var counter = 0;
            var sb = new StringBuilder();

            foreach (var memberProfileForListViewModel in willExpireSoonList)
            {
                try
                {
                    SendRenewMembershipMail(memberProfileForListViewModel);
                    _repository.SetRenewSubscriptionMailSent(memberProfileForListViewModel.Id);
                    sb.AppendLine(string.Format("SUCCESS -- {0} {1} {2} {3}<br />", memberProfileForListViewModel.Id,
                        memberProfileForListViewModel.Lidnr, memberProfileForListViewModel.FullName,
                        memberProfileForListViewModel.Email));
                }
                catch (Exception ex)
                {
                    sb.AppendLine(string.Format("FAIL -- {0} {1} {2}", memberProfileForListViewModel.Id,
                        memberProfileForListViewModel.Lidnr, memberProfileForListViewModel.FullName));
                    LogHelper.Error<Exception>("mail could not be sent", ex);
                }
                counter++;
            }
            model.itemsCount = counter;
            model.MailingList = sb.ToString();

            if (counter > 0)
            {
                // mail for administrator
                var mailto = "benoitgevaert@gmail.com";
                var mailfrom = ConfigurationManager.AppSettings["Mailfrom"];

                var body = sb.ToString();

                var subject = "Deze mensen kregen een remindermail";
                var _emailSender = new EmailSender(Settings.SmtpServer);
                _emailSender.SendEmail(mailfrom, "", new List<string> {mailto}, new List<string>(), subject, body, null,
                    false);
            }
            return View("ReminderMailsView", model);
        }

        public ActionResult SetMembersToSleep()
        {
            var model = new RemindermailsModel();
            var memberlist = _repository.GetAllMembers().OrderBy(p => p.nodeId).ToList();
            var ExpiredList = ViewModelHelper.GetMemberProfilesListViewModel(memberlist)
                .Where(p => p.HasReceivedReminderMail && p.MembershipWilExpireSoon && !p.IsRemoved).ToList();


            var counter = 0;
            var sb = new StringBuilder();
            foreach (var member in ExpiredList)
            {
                if (member.SubscriptionExpiry.AddMonths(1).Ticks < DateTime.Now.Ticks)
                {
                    _repository.SetMembershipToSleep(member.Id);

                    counter++;

                    sb.AppendLine(string.Format("REMOVED -- {0} {1} {2} {3}<br />", member.Id,
                        member.Lidnr, member.FullName, member.SubscriptionExpiry.ToString("dd-MM-yy")));
                }
            }

            model.itemsCount = counter;
            model.MailingList = sb.ToString();


            return View("MembersRemoved", model);
        }

        [OutputCache(Duration = 0, VaryByParam = "*")]
        public JsonResult SetMembershipPaid(int id)
        {
            try
            {
                _repository.SetMemberShipPaid(id);
                var member = _repository.GetMemberById(id);
                SendConfirmationMail(member);

                return Json(new {Success = true}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Success = false}, JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(Duration = 0, VaryByParam = "*")]
        public JsonResult UpdateCredentialsForUser(int id)
        {
            try
            {
                var result = _repository.ChangeCredentials(id);
                if (result.Success)
                {
                    return Json(new {Success = true, result.Message}, JsonRequestBehavior.AllowGet);
                }
                return Json(new {Success = false, result.Message}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Success = false, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(Duration = 0, VaryByParam = "*")]
        public JsonResult SetMembershipToSleep(int id)
        {
            try
            {
                _repository.SetMembershipToSleep(id);
                return Json(new {Success = true}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Success = false}, JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(Duration = 0, VaryByParam = "*")]
        public JsonResult RenewMembership(int id)
        {
            try
            {
                _repository.RenewMembership(id);
                var model = MemberProfileViewModel.GetMemberProfileViewModel(id);

                var returnmessage =
                    String.Format(
                        "<div class=\"row\"><div class=\"info twelve column\"><p>We hebben uw herinschrijving op www.bvlf.org goed ontvangen. " +
                        "<br />Uw lidmaatschap van BVLF is echter pas definitief na betaling<br />" +
                        "van uw inschrijvingsgeld (20€) op rekening<br /><strong>BVLF vzw – Gent BE67 2900 5055 3387</strong>, " +
                        "<br />met de melding (in de vrije mededeling)</p><p><strong> 2015 - {0} - {1}</strong><br />" +
                        "<br />Na de betaling krijgt u van ons een bevestiging. </p></div></div>",
                        model.Id, string.Format("{0} {1}", model.FirstName, model.LastName));

                SendConfirmationMail(model, model.Lidnr);
                return Json(new
                {
                    Success = true,
                    Message = returnmessage
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Success = false}, JsonRequestBehavior.AllowGet);
            }
        }

        private void SendConfirmationMail(MemberProfileViewModel model, string lidnr)
        {
            var _templateRenderer = new TemplateRenderer(HttpContext.Server.MapPath("~/Templates"));

            var mailto = model.Email;
            var mailfrom = ConfigurationManager.AppSettings["Mailfrom"];
            //ConfirmationInschrijving
            var body = _templateRenderer.RenderTemplate("ConfirmationInschrijving.txt", new
            {
                lidnr,
                year = "2015"
            });

            var subject = "Bevestiging van uw aanmelding bij bvlf.org";

            var _emailSender = new EmailSender(Settings.SmtpServer);
            _emailSender.SendEmail(mailfrom, "", new List<string> {mailto}, new List<string>(), subject, body, null,
                false);
        }

        private void SendConfirmationMail(Member member)
        {
            var _templateRenderer = new TemplateRenderer(HttpContext.Server.MapPath("~/Templates"));

            var mailto = member.Email;
            var mailfrom = ConfigurationManager.AppSettings["Mailfrom"];
            //ConfirmationInschrijving
            var body = _templateRenderer.RenderTemplate("ConfirmationPaymentMembership.txt", new
            {
                Naam = ViewModelHelper.GetStringValue(member.getProperty("naam")),
                lidnr = ViewModelHelper.GetStringValue(member.getProperty("Lidnummer"))
            });

            var subject = "Bevestiging van uw herinschrijving";
            var _emailSender = new EmailSender(Settings.SmtpServer);
            _emailSender.SendEmail(mailfrom, "", new List<string> {mailto}, new List<string>(), subject, body, null,
                false);
        }

        private void SendRenewMembershipMail(MemberProfileForListViewModel member)
        {
            var _templateRenderer = new TemplateRenderer(HttpContext.Server.MapPath("~/Templates"));
            var mailto = member.Email;
            var mailfrom = ConfigurationManager.AppSettings["Mailfrom"];

            var body = _templateRenderer.RenderTemplate("ConfirmationRenewInschrijving.txt", new
            {
                naam = member.FullName,
                lidnr = member.Lidnr
            });

            var subject = "Uw lidmaatschap op bvlf.org is bijna verlopen";
            var _emailSender = new EmailSender(Settings.SmtpServer);
            _emailSender.SendEmail(mailfrom, "", new List<string> {mailto}, new List<string>(), subject, body, null,
                false);
        }

        public void EvaluateMembers()
        {
            var memberlist = _repository.GetAllMembers();
            var memberViewModels = ViewModelHelper.GetMemberProfilesListViewModel(memberlist);

            var membersToExpireSoon = memberViewModels.Where(p => p.MembershipWilExpireSoon).ToList();
            var removedMembers = memberViewModels.Where(p => p.IsRemoved).ToList();
            var LatePayments = memberViewModels.Where(p => p.PaymentIsLate).ToList();
        }

        #region Excell Export

        public FileContentResult ExportLedenlijst()
        {
            var filename = string.Format("BvlfLedenlijst_{1}_{0}.xlsx", DateTime.Now.Ticks, DateTime.Now.Year);
            var memberlist = _repository.GetAllMembers();

            var model = MemberProfileListForExortViewModel.GetMemberProfileListForExortViewModel(memberlist);

            var outputstream = ExcellWriterHelper.CreateLedenlijstExportFile(model);
            return File(outputstream, "application/vnd.ms-excel", filename);
        }

        #endregion
    }
}