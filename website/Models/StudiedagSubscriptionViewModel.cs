using System;
using System.Collections.Generic;
using System.Linq;
using bvlf_v2.BOL.Data;
using Umbraco.Core.Models;

namespace bvlf_v2.Models
{
    public class StudiedagSubscriptionViewModel
    {
        public DateTime? SubscriptionDate { get; set; }
        public string Status { get; set; }
        public bool PaymentIsLate { get; set; }
        public bool PaidBySchool { get; set; }
        public List<string> Sessions { get; set; }
        public int SubscriberId { get; set; }
        public int Id { get; set; }

        private static List<string> GetSessions(
            ICollection<StudieDagSubscriptions_Sessions> studieDagSubscriptionsSessions, IEnumerable<IContent> sessions)
        {
            var returnlist = new List<string>();
            foreach (var session in studieDagSubscriptionsSessions)
            {
                var sessie = sessions.FirstOrDefault(p => p.Id == session.SessionId);
                returnlist.Add(sessie.GetValue("at_Nummer").ToString());
            }
            return returnlist;
        }
    }
}