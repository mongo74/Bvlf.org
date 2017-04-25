using System;
using System.Collections.Generic;
using System.Linq;
using bvlf_v2.Models;
using umbraco;

namespace bvlf_v2.Helpers
{
    public class EnumExtentions
    {
        public static CustomListViewModel<int> GetEnumValues<T>() where T : struct, IConvertible
        {
            var returnList = new CustomListViewModel<int>
            {
                ListItems = new List<CustomListItem<int>>()
            };
            var list = Enum.GetValues(typeof (T)).Cast<T>();
            foreach (var gender in list)
            {
                returnList.ListItems.Add(new CustomListItem<int>
                {
                    Value = (int) (object) gender,
                    Text = library.GetDictionaryItem(gender.ToString())
                });
            }
            return returnList;
        }

        public static Dictionary<int, string> GetEnumDictionary<T>()
        {
            return Enum.GetValues(typeof (T))
                .Cast<T>()
                .ToDictionary(t => (int) (object) t, t => library.GetDictionaryItem(t.ToString()));
        }
    }
}