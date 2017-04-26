using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using Umbraco.Core.Logging;

namespace bvlf_v2.BOL.Helpers
{
    public class EmailSender
    {
        private readonly SmtpClient _smtpClient;

        public EmailSender(string smtpServer)
        {
            var mailLogin = Settings.MailUsername;
            var mailPW = Settings.MailPassword;
            _smtpClient = new SmtpClient(smtpServer)
            {
                Credentials = new NetworkCredential(mailLogin, mailPW),
                EnableSsl = false,
                Port = 25
            };
        }

        public void SendEmail(string from, string fromDisplayName, string to, string subject, string body,
            bool ishtml = true)
        {
            SendEmail(from, fromDisplayName, new List<string> {to}, subject, body, ishtml);
        }

        public void SendEmail(string from, string fromDisplayName, List<string> to, string subject, string body,
            bool ishtml = true)
        {
            SendEmail(from, fromDisplayName, to, null, subject, body, null, ishtml);
        }

        //public  bool SendEmail(string from, string fromDisplayName, List<string> to, List<string> cc, string subject, string body, List<string> attachments = null, bool ishtml = true)
        //{
        //    var mail = new MailMessage();

        //    try
        //    {
        //        mail.Subject = subject;
        //        foreach (string email in to)
        //        {
        //            if (!string.IsNullOrWhiteSpace(email))
        //                mail.To.Add(email);
        //        }
        //        if (cc != null)
        //        {
        //            foreach (string email in cc)
        //            {
        //                if (!string.IsNullOrWhiteSpace(email))
        //                    mail.CC.Add(email);
        //            }
        //        }


        //        mail.From = new MailAddress(from, fromDisplayName);
        //        mail.Body = body;
        //        mail.IsBodyHtml = ishtml;

        //        if (attachments != null)
        //        {
        //            foreach (var attachment in attachments)
        //            {
        //                mail.Attachments.Add(new Attachment(attachment));
        //            }
        //        }
        //     //   new SmtpClient().Send(mail);

        //        _smtpClient.Send(mail);

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Add(LogTypes.Error, -1, string.Format("ConfirmationMail could not be sent - {0}", ex.ToString()));

        //        return false;
        //    }
        //    return true;
        //}

        public void SendEmail(string from, string fromDisplayName, List<string> to, List<string> cc, string subject,
            string body, List<string> attachments = null, bool ishtml = true)
        {
            var mail = new MailMessage();

            try
            {
                mail.Subject = subject;
                foreach (var email in to)
                {
                    if (!string.IsNullOrWhiteSpace(email))
                        mail.To.Add(email);
                }
                if (cc != null)
                {
                    foreach (var email in cc)
                    {
                        if (!string.IsNullOrWhiteSpace(email))
                            mail.CC.Add(email);
                    }
                }

                mail.From = new MailAddress(from, fromDisplayName);
                mail.Body = body;
                mail.IsBodyHtml = ishtml;

                if (attachments != null)
                {
                    foreach (var attachment in attachments)
                    {
                        mail.Attachments.Add(new Attachment(attachment));
                    }
                }
                _smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                var smptcredentials = string.Format("{0}", _smtpClient.Host);
                //   Log.Add(LogTypes.Error, -1, string.Format("ConfirmationMail could not be sent - {0} - {1}", ex,smptcredentials));
                //   LogHelper.Error(string.Format("ConfirmationMail could not be sent - {0} - {1}", ex, smptcredentials), ex);
                LogHelper.Error<Exception>(
                    string.Format("ConfirmationMail could not be sent - {0} - {1} - {2}", ex, smptcredentials,
                        string.Join("|", to.ToArray())), ex);
            }
        }
    }
}