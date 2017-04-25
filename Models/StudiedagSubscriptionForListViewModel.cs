using System.Collections.Generic;
using System.Linq;
using bvlf_v2.BOL.Data;
using bvlf_v2.BOL.Repositories;
using bvlf_v2.Helpers;
using Umbraco.Core.Models;

namespace bvlf_v2.Models
{
    public class StudiedagSubscriptionForListViewModel : StudiedagSubscriptionViewModel
    {
        public string Email { get; set; }
        public string Lidnr { get; set; }
        public string School { get; set; }
        public string SchoolLocation { get; set; }
        public string FullName { get; set; }

        public static StudiedagSubscriptionForListViewModel GetStudiedagSubscriptionForListViewModel(
            StudieDagSubscription subscription, IEnumerable<IContent> sessions)
        {
            var _repository = new MembershipRepository();
            ;
            var member = _repository.GetMemberById(subscription.SubscriberId);

            return new StudiedagSubscriptionForListViewModel
            {
                Id = subscription.Id,
                SubscriberId = subscription.SubscriberId,
                Email = member.Email,
                School = ViewModelHelper.GetStringValue(member.getProperty("naamschool_1")),
                SchoolLocation = ViewModelHelper.GetStringValue(member.getProperty("plaatsschool_1")),
                Lidnr = ViewModelHelper.GetStringValue(member.getProperty("lidnr")),
                SubscriptionDate = subscription.SubscriptionDate,
                Status = ((SubscriptionStatus) subscription.SubscriptionStatus).ToString(),
                FullName =
                    string.Format("{0} {1}", ViewModelHelper.GetStringValue(member.getProperty("naam")).ToUpper(),
                        ViewModelHelper.GetStringValue(member.getProperty("voornaam"))),
                Sessions = GetSessions(subscription.StudieDagSubscriptions_Sessions, sessions),
                PaidBySchool = subscription.PaidBySchool ?? false
                //Enum.Parse(typeof(BvlfMemberStatus), ViewModelHelper.GetStringValue(member.getProperty("bvlfMemberStatus"))).ToString(),
            };
        }

        private static List<string> GetSessions(
            ICollection<StudieDagSubscriptions_Sessions> studieDagSubscriptionsSessions, IEnumerable<IContent> sessions)
        {
            return studieDagSubscriptionsSessions
                .Select(session => sessions.FirstOrDefault(p => p.Id == session.SessionId))
                .Select(sessie => sessie.GetValue("at_Nummer").ToString()).ToList();
        }
    }
}