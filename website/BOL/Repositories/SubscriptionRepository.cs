using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Xml;
using bvlf_v2.BOL.Data;
using bvlf_v2.Models;

namespace bvlf_v2.BOL.Repositories
{
    public class SubscriptionRepository
    {
        private readonly bvlf_org_v2Entities _context;

        public SubscriptionRepository()
        {
            _context = new bvlf_org_v2Entities();
        }

        public void SaveSubscription(StudieDagSubscription sub)
        {
            _context.StudieDagSubscriptions.Add(sub);
            _context.SaveChanges();
        }

        public void UpdateSubscription(StudieDagSubscription sub)
        {
            var oldSub = GetSubscriptionBySubscriberId(sub.SubscriberId);
            var oc = ((IObjectContextAdapter) _context).ObjectContext;
            oldSub.StudieDagSubscriptions_Sessions.ToList().ForEach(oc.DeleteObject);
            _context.SaveChanges();
            sub.StudieDagSubscriptions_Sessions.ToList().ForEach(r => oldSub.StudieDagSubscriptions_Sessions.Add(r));
            _context.SaveChanges();
        }

        public Dictionary<int, int> GetCurrentSubscriptionsCount()
        {
            var subscriptions = _context.StudieDagSubscriptions_Sessions.GroupBy(i => i.SessionId);
            if (subscriptions != null && subscriptions.Any())
                return subscriptions.ToDictionary(grp => grp.Key, grp => grp.Count());
            return new Dictionary<int, int>();
        }

        public StudieDagSubscription GetSubscriptionBySubscriberId(int subscriberid)
        {
            return
                _context.StudieDagSubscriptions.Where(p => p.Year == Settings.StudiedagYear)
                    .FirstOrDefault(p => p.SubscriberId == subscriberid);
        }

        public StudieDagSubscription GetSubscriptionBySubscriptionId(int subscriptionid)
        {
            return
                _context.StudieDagSubscriptions.Where(p => p.Year == Settings.StudiedagYear)
                    .FirstOrDefault(p => p.Id == subscriptionid);
        }

        public List<StudieDagSubscription> GetAllSubscriptions()
        {
            return _context.StudieDagSubscriptions.Where(p => p.Year == Settings.StudiedagYear).ToList();
        }

        public void SetSubscriberBetaald(int subscriptionid)
        {
            var sub = GetSubscriptionBySubscriptionId(subscriptionid);
            var oc = ((IObjectContextAdapter) _context).ObjectContext;
            sub.SubscriptionStatus = (int) SubscriptionStatus.Betaald;
            sub.PaymentDate = DateTime.Now;
            _context.SaveChanges();
        }

        public void CancelSubscription(int subscriberid)
        {
            var sub = GetSubscriptionBySubscriptionId(subscriberid);
            var oc = ((IObjectContextAdapter) _context).ObjectContext;
            sub.StudieDagSubscriptions_Sessions.ToList().ForEach(oc.DeleteObject);
            _context.SaveChanges();
            oc.DeleteObject(sub);

            _context.SaveChanges();
        }

        public Dictionary<int, XmlDocument> GetSessionsAsXml(List<int> sessionids)
        {
            var returnval = new Dictionary<int, XmlDocument>();
            // context.Orders.Where(o => o.Items.Any(i => selectedIds.Contains(i.ItemId)))
            var list = _context.cmsContentXmls.Where(p => sessionids.Contains(p.nodeId));
            foreach (var item in list)
            {
                var doc = new XmlDocument();
                doc.LoadXml(item.xml);
                returnval.Add(item.nodeId, doc);
            }
            return returnval;
        }
    }
}