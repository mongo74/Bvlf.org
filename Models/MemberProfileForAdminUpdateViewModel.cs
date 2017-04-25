using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using bvlf_v2.Helpers;

namespace bvlf_v2.Models
{
    public class MemberProfileForAdminUpdateViewModel
    {
        public MemberProfileForAdminUpdateViewModel()
        {
            rbGenderOptions = EnumExtentions.GetEnumValues<Gender>();
            MemberStatusOptions = EnumExtentions.GetEnumDictionary<BvlfMemberStatus>();
            MemberprofileOptions = EnumExtentions.GetEnumDictionary<BvlfMemberProfile>();
            School1 = new SchoolProfileViewModel();
            School2 = new SchoolProfileViewModel();
        }

        public int Id { get; set; }
        public int SelectedGender { get; set; }

        [Required(ErrorMessage = "*")]
        public CustomListViewModel<int> rbGenderOptions { get; set; }

        public int MemberStatus { get; set; }
        public Dictionary<int, string> MemberStatusOptions { get; set; }
        public int MemberProfile { get; set; }

        [DisplayName("Profiel")]
        public Dictionary<int, string> MemberprofileOptions { get; set; }

        [Required(ErrorMessage = "Vergeet je voornaam niet")]
        [DisplayName("Voornaam *")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Vergeet je naam niet")]
        [DisplayName("Naam")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Vergeet je emailadres niet")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
            ErrorMessage = "Dit is geen geldig emailadres")]
        [DisplayName("Email - ook uw login *")]
        public string Email { get; set; }

        public string Lidnr { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Adres *")]
        public string Street { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("nr *")]
        public string Nr { get; set; }

        [DisplayName("Bus")]
        public string Box { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Postcode *")]
        public string Zip { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Plaats *")]
        public string Location { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Land *")]
        public string Country { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Tel *")]
        public string Tel { get; set; }

        [DisplayName("GSM")]
        public string Mobile { get; set; }

        public string FipfNr { get; set; }
        public SchoolProfileViewModel School1 { get; set; }
        public SchoolProfileViewModel School2 { get; set; }
    }
}