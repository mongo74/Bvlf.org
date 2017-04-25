using System;
using System.Configuration;

namespace bvlf_v2.BOL
{
    public static class Settings
    {
        public static int LatePaymentInDays
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["LatePaymentInDays"]); }
        }

        public static int ReminderInDays
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["ReminderInDays"]); }
        }

        public static int SubscriptionExpiry
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["SubscriptionExpiry"]); }
        }

        public static int StudiedagYear
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["StudiedagYear"]); }
        }

        #region SMTP - MAIL

        public static string MailFrom
        {
            get { return ConfigurationManager.AppSettings["MailFrom"]; }
        }

        public static string MailTo
        {
            get { return ConfigurationManager.AppSettings["MailTo"]; }
        }

        public static string SmtpServer
        {
            get { return ConfigurationManager.AppSettings["SmtpServer"]; }
        }

        public static string MailUsername
        {
            get { return ConfigurationManager.AppSettings["MailUsername"]; }
        }

        public static string MailPassword
        {
            get { return ConfigurationManager.AppSettings["MailPassword"]; }
        }

        #endregion
    }
}