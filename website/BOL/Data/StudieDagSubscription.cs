//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace bvlf_v2.BOL.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class StudieDagSubscription
    {
        public StudieDagSubscription()
        {
            this.StudieDagSubscriptions_Sessions = new HashSet<StudieDagSubscriptions_Sessions>();
        }
    
        public int Id { get; set; }
        public int Year { get; set; }
        public int SubscriberId { get; set; }
        public Nullable<bool> PaidBySchool { get; set; }
        public System.DateTime SubscriptionDate { get; set; }
        public int SubscriptionStatus { get; set; }
        public int MessageStatus { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
    
        public virtual ICollection<StudieDagSubscriptions_Sessions> StudieDagSubscriptions_Sessions { get; set; }
        public virtual cmsMember cmsMember { get; set; }
    }
}