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
    
    public partial class cmsMember
    {
        public cmsMember()
        {
            this.StudieDagSubscriptions = new HashSet<StudieDagSubscription>();
        }
    
        public int nodeId { get; set; }
        public string Email { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
    
        public virtual ICollection<StudieDagSubscription> StudieDagSubscriptions { get; set; }
        public virtual cmsContentXml cmsContentXml { get; set; }
    }
}
