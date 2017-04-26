using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using bvlf_v2.BOL;
using bvlf_v2.BOL.Helpers;
using bvlf_v2.BOL.Repositories;
using bvlf_v2.Models;
using Umbraco.Web.Mvc;

namespace bvlf_v2.Controllers
{
    /// <summary>
    ///     Summary description for MembershipController
    /// </summary>
    public class MembershipController : SurfaceController
    {
        private const string ck_Attempts = "loginAttempts";
        private const string ck_Message = "loginMessage";
        private readonly EmailSender _emailSender;
        private readonly MembershipRepository _repository;

        public MembershipController()
        {
            _emailSender = new EmailSender(Settings.SmtpServer);
            _repository = new MembershipRepository();
        }

        [ChildActionOnly]
        public ActionResult Login()
        {
            var cacheValue = 0;

            if (System.Web.HttpContext.Current.Cache[ck_Attempts] != null)
            {
                cacheValue = (int) HttpContext.Cache[ck_Attempts];
            }
            else
            {
                HttpContext.Cache[ck_Attempts] = cacheValue;
            }

            var cacheStringValue = string.Empty;
            if (System.Web.HttpContext.Current.Cache[ck_Message] != null)
            {
                cacheStringValue = (string) HttpContext.Cache[ck_Message];
            }
            else
            {
                HttpContext.Cache[ck_Message] = cacheStringValue;
            }


            var model = new LoginViewModel();
            return PartialView("Login", model);
        }

        [HttpPost]
        public ActionResult HandleLogin(LoginViewModel viewmodel)
        {
            var cacheValue = HttpContext.Cache[ck_Attempts] ?? 0;

            var langroot = Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);
            var langCookie = new HttpCookie("LangCookie", langroot);
            langCookie.Expires.AddDays(365);
            Response.Cookies.Add(langCookie);

            if (!ModelState.IsValid)
            {
                //    HandleFailedLoginCache(viewmodel, cacheValue);
                return CurrentUmbracoPage();
            }
            if (!Membership.ValidateUser(viewmodel.Login, viewmodel.Password))
            {
                //    HandleFailedLoginCache(viewmodel, cacheValue);
                return CurrentUmbracoPage();
            }
            var isAllowed = _repository.MemberIsAllowed(viewmodel.Login);
            if (!isAllowed)
            {
                return CurrentUmbracoPage();
            }

            FormsAuthentication.SetAuthCookie(viewmodel.Login, viewmodel.RememberMe);
            //   HttpContext.Cache.Remove(ck_Attempts);
            //   HttpContext.Cache.Remove(ck_Message);
            return Redirect(Request.QueryString["ReturnUrl"] ?? string.Format("/{0}.aspx", langroot));
        }

        private void HandleFailedLoginCache(LoginViewModel viewmodel, object cacheValue)
        {
            cacheValue = (int) cacheValue + 1;
            viewmodel.FailedAttempts = (int) cacheValue;
            HttpContext.Cache[ck_Attempts] = cacheValue;

            if ((int) cacheValue > 1)
            {
                HttpContext.Cache[ck_Message] = viewmodel.FailedAttemptsMessage = BuildRedirectToLoginMessage();
            }
        }

        private static string BuildRedirectToLoginMessage()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("<br /><h3>Het lijkt erop dat...</h3>"));
            sb.AppendLine(string.Format("<p>u behoorlijk worstelt met uw login op deze site.</p>"));
            sb.AppendLine(string.Format("<p></p>"));
            sb.AppendLine(string.Format(@"<p><a href=\"">dit is de plek waar een link moet komen</a></p>"));

            return sb.ToString();
        }

        public ActionResult ChangePassword()
        {
            var model = new ChangePasswordViewModel();
            if (HttpContext.Cache["pwChanged"] != null)
            {
                model.PasswordHasChanged = bool.Parse(HttpContext.Cache["pwChanged"].ToString());
            }
            HttpContext.Cache.Remove("pwChanged");
            return PartialView("ChangePW", model);
        }

        /// <summary>
        ///     Change password Handler
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Action result - CurrentUmbracoPage</returns>
        [HttpPost]
        public ActionResult HandleChangePassword(ChangePasswordViewModel model)
        {
            ValidateChangePWModel(model);
            if (ModelState.IsValid)
            {
                if (_repository.ChangePassword(model))
                {
                    HttpContext.Cache["pwChanged"] = model.PasswordHasChanged = true;
                    return CurrentUmbracoPage();
                }
                ModelState.AddModelError("OldPassword", "Oud paswoord is niet correct");
                return CurrentUmbracoPage();
            }
            return CurrentUmbracoPage();
        }

        /// <summary>
        ///     Validate Password
        ///     Adds error to Modelstate
        /// </summary>
        /// <param name="model"></param>
        private void ValidateChangePWModel(ChangePasswordViewModel model)
        {
            if (model.Password != model.ComparePassword)
            {
                ModelState.AddModelError("Password", "Paswoorden moeten gelijk zijn");
            }
        }

        /// <summary>
        ///     Signout handler
        /// </summary>
        /// <returns></returns>
        public ActionResult HandleSignout()
        {
            var langCookie = Request.Cookies["LangCookie"];
            var langroot = langCookie != null ? langCookie.Value : "nl";
            Session.Clear();
            FormsAuthentication.SignOut();
            return Redirect(string.Format("/{0}.aspx", langroot));
        }

        /// <summary>
        ///     Loads the registration form into the page View
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult LoadRegister()
        {
            var model = MemberProfileViewModel.GetMemberProfileViewModel();
            return PartialView("SubscribeForm", model);
        }

        /// <summary>
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Action result - CurrentUmbracoPage</returns>
        [HttpPost]
        public ActionResult Register(MemberProfileViewModel model)
        {
            ValidateRegisterForm(model);

            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            var lidnr = _repository.CreateNewMember(model);
            model.MemberHasRegistered = true;
            TempData["Status"] = "registered";
            TempData["lidnr"] = lidnr;
            TempData["naam"] = model.LastName;

            // Send confirmation Mail
            SendConfirmationMail(model, lidnr);
            return RedirectToCurrentUmbracoPage();
        }

        /// <summary>
        ///     Sends the InschrijvingsConfirmation mail
        ///     Calls the emailSender Class
        /// </summary>
        /// <param name="model">MemberProfile Viewmodel</param>
        /// <param name="lidnr">Newly created Lidnr</param>
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

            _emailSender.SendEmail(mailfrom, "", new List<string> {mailto}, new List<string>(), subject, body, null,
                false);
        }

        /// <summary>
        ///     Validates the register form
        /// </summary>
        /// <param name="model"></param>
        private void ValidateRegisterForm(MemberProfileViewModel model)
        {
            if (_repository.MemberExists(model.Email))
            {
                ModelState.AddModelError("Email", "Dit email adres bestaat al. Gebruik een ander");
            }

            if (model.Passwoord != model.PasswoordCompare)
            {
                ModelState.AddModelError("Passwoord", "Paswoorden moeten gelijk zijn");
            }

            if (!model.School1.SchoolIsObligatory) return;
            if (string.IsNullOrEmpty(model.School1.SchoolNaam))
            {
                ModelState.AddModelError("School1.SchoolNaam", "*");
            }
        }

        /// <summary>
        ///     Validates the updated form
        /// </summary>
        /// <param name="model"></param>
        private void ValidateUpdateForm(MemberProfileViewModel model)
        {
            if (!model.School1.SchoolIsObligatory) return;
            if (string.IsNullOrEmpty(model.School1.SchoolNaam))
            {
                ModelState.AddModelError("School1.SchoolNaam", "*");
            }
        }

        [ChildActionOnly]
        public ActionResult LoadPersonalData(int id)
        {
            var model = MemberProfileViewModel.GetMemberProfileViewModel(id);
            return PartialView("UpdatePersonalInfo", model);
        }

        [HttpPost]
        public ActionResult UpdatePersonalInfo(MemberProfileViewModel model)
        {
            ValidateUpdateForm(model);

            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }
            _repository.UpdateMember(model);
            return RedirectToUmbracoPage(1052);
        }

        public ActionResult LoadMemberList()
        {
            // new one, with the javascript, ajax calls and filters
            var model = new MemberSearchViewModel();
            return PartialView("MemberSearch", model);
        }

        public ActionResult ForgotPassword()
        {
            var model = new ForgotPasswordViewModel();

            if (HttpContext.Cache["mailsent"] != null)
            {
                model.MailSent = bool.Parse(HttpContext.Cache["mailsent"].ToString());
            }
            HttpContext.Cache.Remove("mailsent");

            return PartialView("ForgotPassword", model);
        }

        [HttpPost]
        public ActionResult HandleForgotPassword(ForgotPasswordViewModel model)
        {
            var langroot = Thread.CurrentThread.CurrentCulture.ToString().Substring(0, 2);
            var langCookie = new HttpCookie("LangCookie", langroot);
            langCookie.Expires.AddDays(365);
            Response.Cookies.Add(langCookie);

            ValidateForgotPw(model);

            if (!ModelState.IsValid) return CurrentUmbracoPage();

            var pw = _repository.RetrievePassword(model.Email);
            SendForgotPWMail(model.Email, pw);
            model.MailSent = true;
            HttpContext.Cache["mailsent"] = model.MailSent = true;
            return RedirectToCurrentUmbracoPage();
        }

        private void SendForgotPWMail(string email, string pw)
        {
            var subject = "Uw paswoord voor bvlf.org";
            var emailSender = new EmailSender(Settings.SmtpServer);
            emailSender.SendEmail(Settings.MailFrom, "bvlf.org", email, subject, pw);
        }

        private void ValidateForgotPw(ForgotPasswordViewModel model)
        {
            if (!_repository.MemberExists(model.Email))
            {
                ModelState.AddModelError("Email", "Dit emailadres werd niet terug gevonden");
            }
        }
    }
}