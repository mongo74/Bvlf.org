using System.ComponentModel;

namespace bvlf_v2.Models
{
    public class SchoolProfileViewModel
    {
        public SchoolProfileViewModel()
        {
            SchoolIsObligatory = false;
        }

        public SchoolProfileViewModel(bool schoolisobligatory)
        {
            SchoolIsObligatory = schoolisobligatory;
        }

        public bool SchoolIsObligatory { get; set; }

        [DisplayName("Naam")]
        public string SchoolNaam { get; set; }

        public string Street { get; set; }
        public string Nr { get; set; }
        public string Box { get; set; }
        public string Zip { get; set; }
        public string Location { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
    }
}