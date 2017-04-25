using System.Collections.Generic;
using System.Linq;
using System.Xml;
using bvlf_v2.BOL.Data;
using bvlf_v2.BOL.Objects;
using bvlf_v2.Models;
using umbraco.cms.businesslogic.property;

namespace bvlf_v2.Helpers
{
    public class ViewModelHelper
    {
        public static string GetStringValue(Property property)
        {
            return property != null
                ? (!string.IsNullOrEmpty(property.Value.ToString()) ? property.Value.ToString() : "")
                : string.Empty;
        }

        public static List<MemberProfileForListViewModel> GetMemberProfilesListViewModel(List<cmsMember> memberlist)
        {
            return memberlist.Select(MemberProfileForListViewModel.GetMemberProfileForListViewModelByMember).ToList();
        }

        public static MemberProfileForListViewModel GetMemberProfileForListViewModel(cmsMember member)
        {
            return MemberProfileForListViewModel.GetMemberProfileForListViewModelByMember(member);
        }

        private static List<string> GetMySessions(IEnumerable<StudieDagSubscriptions_Sessions> mysessions,
            Dictionary<int, XmlDocument> xmlSessions)
        {
            return
                mysessions.Select(
                    session =>
                        xmlSessions.FirstOrDefault(p => p.Key == session.SessionId)
                            .Value.GetElementsByTagName("at_Nummer")[0].InnerText).ToList();
        }

        public static List<StudiedagSubscriptionForListViewModel> GetInschrijvingenLijstViewModelByEF(
            List<StudieDagSubscription> subscriptions,
            Dictionary<int, XmlDocument> xmlSessions)
        {
            var returnlist = new List<StudiedagSubscriptionForListViewModel>();

            foreach (var subscription in subscriptions)
            {
                var mysessions = subscription.StudieDagSubscriptions_Sessions;

                if (subscription.cmsMember != null && subscription.cmsMember.cmsContentXml != null &&
                    subscription.cmsMember.cmsContentXml.xml != null)
                {
                    var member =
                        SerializationTools.DeSerializeString<cmsMemberXml>(subscription.cmsMember.cmsContentXml.xml);
                    if (member != null)
                    {
                        var fullname = string.Format("{0} {1}", member.FirstName, member.Name);
                        var email = member.Email;
                        returnlist.Add(new StudiedagSubscriptionForListViewModel
                        {
                            School = member.School1Naam,
                            FullName = fullname,
                            Email = email,
                            Lidnr = member.LidNr,
                            Status = ((SubscriptionStatus) subscription.SubscriptionStatus).ToString(),
                            Sessions = GetMySessions(mysessions, xmlSessions),
                            SubscriberId = subscription.SubscriberId,
                            SubscriptionDate = subscription.SubscriptionDate,
                            Id = subscription.Id,
                            PaidBySchool = subscription.PaidBySchool ?? false
                        });
                    }
                }
            }
            return returnlist;
        }

        public static List<StudiedagSubscriptionForExportViewModel> GetInschrijvingenLijstForExportViewModelByEF(
            List<StudieDagSubscription> subscriptions, Dictionary<int, XmlDocument> xmlSessions)
        {
            var returnList = new List<StudiedagSubscriptionForExportViewModel>();

            foreach (var subscription in subscriptions)
            {
                if (subscription.cmsMember != null && subscription.cmsMember.cmsContentXml != null &&
                    subscription.cmsMember.cmsContentXml.xml != null)
                {
                    var mysessions =
                        subscription.StudieDagSubscriptions_Sessions;
                    var member =
                        SerializationTools.DeSerializeString<cmsMemberXml>(subscription.cmsMember.cmsContentXml.xml);

                    returnList.Add(new StudiedagSubscriptionForExportViewModel
                    {
                        Id = subscription.Id,
                        Status = ((SubscriptionStatus) subscription.SubscriptionStatus).ToString(),
                        SubscriptionDate = subscription.SubscriptionDate,
                        SubscriberId = subscription.SubscriberId,
                        Sessions = GetMySessions(mysessions, xmlSessions),
                        Member = member,
                        PaidBySchool = subscription.PaidBySchool ?? false
                    });
                }
            }

            return new List<StudiedagSubscriptionForExportViewModel>(returnList.OrderBy(p => p.Member.Name));
        }
    }
}