using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using bvlf_v2.Helpers;

namespace bvlf_v2.Models
{
    public class MemberSearchViewModel
    {
        [DisplayName(@"Search by")]
        public string SearchCriterium { get; set; }

        public int MemberStatus { get; set; }

        [DisplayName("Lidmaatschap status")]
        public Dictionary<int, string> MemberStatusList
        {
            get
            {
                var returnlist = EnumExtentions.GetEnumDictionary<BvlfMemberStatus>();
                returnlist.Add(0, "--");
                //returnlist.Where(p => p.Value.ToLower() == "studiedag").de
                returnlist.OrderBy(p => p.Key);

                var itemToRemove = returnlist.Single(r => r.Value.ToLower() == "studiedag");
                returnlist.Remove(itemToRemove.Key);

                return returnlist;
            }
        }
    }
}