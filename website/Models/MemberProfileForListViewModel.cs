using System;
using System.Xml;
using bvlf_v2.BOL;
using bvlf_v2.BOL.Data;

namespace bvlf_v2.Models
{
    public class MemberProfileForListViewModel
    {
        public string Email { get; set; }
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Lidnr { get; set; }
        public string School { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public DateTime SubscriptionExpiry { get; set; }
        public string Status { get; set; }
        public bool PaymentIsLate { get; set; }
        public bool MembershipWilExpireSoon { get; set; }
        public bool IsRemoved { get; set; }
        public bool HasReceivedReminderMail { get; set; }
        public bool IsToExpire { get; set; }
        public string SubscriptionDateString { get; set; }
        public string SubscriptionExpiryString { get; set; }

        public static MemberProfileForListViewModel GetMemberProfileForListViewModelByMember(cmsMember member)
        {
            var doc = new XmlDocument();
            doc.LoadXml(member.cmsContentXml.xml);
            var fullname = string.Format("{0} {1}", doc.GetElementsByTagName("voornaam")[0].InnerText,
                doc.GetElementsByTagName("naam")[0].InnerText);
            var email = doc.GetElementsByTagName("node")[0].Attributes["email"].Value;

            //         var expirationDate = DateTime.Parse(doc.GetElementsByTagName("subscriptionExpiration")[0].InnerText);    

            DateTime dateValue;
            var expirationDate = DateTime.Now;
            var breakFunction = false;
            if (DateTime.TryParse(doc.GetElementsByTagName("subscriptionExpiration")[0].InnerText, out dateValue))
                expirationDate = dateValue;
            else
            {
                breakFunction = true;
            }


            if (!breakFunction)
            {
                var subscriptionDate = DateTime.Parse(doc.GetElementsByTagName("subscriptionDate")[0].InnerText);

                var status =
                    doc.GetElementsByTagName("bvlfMemberStatus")[0].InnerText.Replace("<![CDATA[", "")
                        .Replace("]]>", "");

                var paymentIsLate = status.ToUpper() == "INGESCHREVEN" &&
                                    expirationDate.AddYears(-1).AddDays(Settings.LatePaymentInDays).Ticks <=
                                    DateTime.Now.Ticks;

                var Memberremoved = status.ToUpper() == "VERWIJDERD";
                var willExpireSoon = expirationDate.AddDays(0 - Settings.SubscriptionExpiry).Ticks <=
                                     DateTime.Now.Ticks;

                var school = doc.GetElementsByTagName("naamschool_1")[0].InnerText;
                var lidnr = doc.GetElementsByTagName("lidnr")[0].InnerText;

                var hasReceivedREnewalMail = "0";
                var elemList = doc.GetElementsByTagName("renewalSubscriptionReminder");
                if (elemList != null && elemList.Count > 0)
                {
                    hasReceivedREnewalMail = doc.GetElementsByTagName("renewalSubscriptionReminder")[0].InnerText;
                }

                return new MemberProfileForListViewModel
                {
                    Email = email,
                    FullName = fullname,
                    Id = member.nodeId,
                    Lidnr = lidnr,
                    School = school,
                    SubscriptionDate = subscriptionDate,
                    SubscriptionExpiry = expirationDate,
                    MembershipWilExpireSoon = willExpireSoon,
                    PaymentIsLate = paymentIsLate,
                    Status = status,
                    IsRemoved = Memberremoved,
                    HasReceivedReminderMail = hasReceivedREnewalMail == "1",
                    SubscriptionDateString = subscriptionDate.ToString("dd/MM/yyyy"),
                    SubscriptionExpiryString = expirationDate.ToString("dd/MM/yyyy")
                };
            }
            return null;
        }
    }
}