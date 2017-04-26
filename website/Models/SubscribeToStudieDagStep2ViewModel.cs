using System.ComponentModel;
using bvlf_v2.Helpers;

namespace bvlf_v2.Models
{
    public class SubscribeToStudieDagStep2ViewModel
    {
        public CheckBoxListViewModel<string> cbSessionGroup1List { get; set; }
        public string SelectedGroup1 { get; set; }
        public CheckBoxListViewModel<string> cbSessionGroup2List { get; set; }
        public string SelectedGroup2 { get; set; }
        public CheckBoxListViewModel<string> cbSessionGroup3List { get; set; }
        public string SelectedGroup3 { get; set; }
        public CheckBoxListViewModel<string> cbSessionGroup4List { get; set; }
        public string SelectedGroup4 { get; set; }
        public CheckBoxListViewModel<string> cbSessionGroup5List { get; set; }
        public string SelectedGroup5 { get; set; }
        public CheckBoxListViewModel<string> cbSessionGroup6List { get; set; }
        public string SelectedGroup6 { get; set; }
        public CheckBoxListViewModel<string> cbSessionGroup7List { get; set; }
        public string SelectedGroup7 { get; set; }
        public CheckBoxListViewModel<string> cbSessionGroup8List { get; set; }
        public string SelectedGroup8 { get; set; }
        public CheckBoxListViewModel<string> cbSessionGroup9List { get; set; }
        public string SelectedGroup9 { get; set; }
        public CheckBoxListViewModel<string> cbSessionGroup10List { get; set; }
        public string SelectedGroup10 { get; set; }
        public CheckBoxListViewModel<string> cbSessionGroup11List { get; set; }
        public string SelectedGroup11 { get; set; }
        public CheckBoxListViewModel<string> cbSessionGroup12List { get; set; }
        public string SelectedGroup12 { get; set; }
        public CheckBoxListViewModel<string> cbSessionGroup13List { get; set; }
        public string SelectedGroup13 { get; set; }
        public bool isUpdate { get; set; }
        public int SubscriberId { get; set; }

        [DisplayName("Betaald door de school")]
        public bool PaidBySchool { get; set; }

        [DisplayName("Ik heb de inschrijvingsvoorwaarden gelezen en ga ermee akkoord")]
        public bool HasReadConditions { get; set; }
    }
}