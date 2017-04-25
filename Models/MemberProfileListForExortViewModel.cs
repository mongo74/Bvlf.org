using System.Collections.Generic;
using System.Linq;
using bvlf_v2.BOL.Data;
using bvlf_v2.BOL.Objects;
using bvlf_v2.Helpers;

namespace bvlf_v2.Models
{
    public class MemberProfileListForExortViewModel
    {
        public List<cmsMemberXml> Ledenlijst { get; set; }

        public static MemberProfileListForExortViewModel GetMemberProfileListForExortViewModel(
            List<cmsMember> memberlist)
        {
            var model = new MemberProfileListForExortViewModel
            {
                Ledenlijst =
                    memberlist.Select(
                        cmsMember => SerializationTools.DeSerializeString<cmsMemberXml>(cmsMember.cmsContentXml.xml))
                        .ToList()
            };
            return model;
        }
    }
}