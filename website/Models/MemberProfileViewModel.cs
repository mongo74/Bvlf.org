using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using bvlf_v2.BOL;
using bvlf_v2.BOL.Repositories;
using bvlf_v2.Helpers;
using umbraco.cms.businesslogic.member;

namespace bvlf_v2.Models
{
    public class MemberProfileViewModel : MemberProfileForAdminUpdateViewModel
    {
        public MemberProfileViewModel()
        {
            rbGenderOptions = EnumExtentions.GetEnumValues<Gender>();
            MemberStatusOptions = EnumExtentions.GetEnumDictionary<BvlfMemberStatus>();
            MemberprofileOptions = EnumExtentions.GetEnumDictionary<BvlfMemberProfile>();
            School1 = new SchoolProfileViewModel(true);
            School2 = new SchoolProfileViewModel();
        }

        private string Title { get; set; }

        [DisplayName("Paswoord **")]
        [Required(ErrorMessage = "*")]
        [RegularExpression(@"^.*(?=.{8,})(?=.*[A-Z])(?=.*[\d\W]).*$",
            ErrorMessage =
                "Paswoord moet minstens 8 tekens lang zijn, 1 hoofdletter en 1 cijfer bevatten en 1 speciaal teken")]
        public string Passwoord { get; set; }

        [DisplayName("Bevestig paswoord **")]
        [Required(ErrorMessage = "*")]
        [RegularExpression(@"^.*(?=.{8,})(?=.*[A-Z])(?=.*[\d\W]).*$",
            ErrorMessage =
                "Paswoord moet minstens 8 tekens lang zijn, 1 hoofdletter en 1 cijfer bevatten en 1 speciaal teken")]
        public string PasswoordCompare { get; set; }

        public bool MemberHasRegistered { get; set; }
        public string ExpirationDate { get; set; }
        public string DaysToExpiration { get; set; }
        public bool MembershipToExpireSoon { get; set; }

        public static MemberProfileViewModel GetMemberProfileViewModel()
        {
            var model = new MemberProfileViewModel {rbGenderOptions = {SelectedValue = 1}};
            return model;
        }

        public static MemberProfileViewModel GetMemberProfileViewModel(int id)
        {
            var model = new MemberProfileViewModel();
            var _repository = new MembershipRepository();
            var member = _repository.GetMemberById(id);

            model.Email = member.LoginName;
            model.Id = id;

            model.LastName = ViewModelHelper.GetStringValue(member.getProperty("naam"));
            model.FirstName = ViewModelHelper.GetStringValue(member.getProperty("voornaam"));
            model.Street = ViewModelHelper.GetStringValue(member.getProperty("straat"));
            model.Nr = ViewModelHelper.GetStringValue(member.getProperty("nr"));
            model.Box = ViewModelHelper.GetStringValue(member.getProperty("bus"));
            model.Zip = ViewModelHelper.GetStringValue(member.getProperty("postcode"));
            model.Location = ViewModelHelper.GetStringValue(member.getProperty("plaats"));
            model.Tel = ViewModelHelper.GetStringValue(member.getProperty("tel"));
            model.Mobile = ViewModelHelper.GetStringValue(member.getProperty("mobile"));
            model.Country = ViewModelHelper.GetStringValue(member.getProperty("country"));
            model.SelectedGender =
                (int) Enum.Parse(typeof (Gender), ViewModelHelper.GetStringValue(member.getProperty("gender")));
            model.Passwoord = model.PasswoordCompare = member.Password;

            //   model.MemberStatus = (int)Enum.Parse(typeof(BvlfMemberStatus), ViewModelHelper.GetStringValue(member.getProperty("bvlfMemberStatus")));
            model.MemberProfile =
                (int)
                    Enum.Parse(typeof (BvlfMemberProfile), ViewModelHelper.GetStringValue(member.getProperty("profiel")));

            model.Lidnr = ViewModelHelper.GetStringValue(member.getProperty("lidnr"));
            model.FipfNr = ViewModelHelper.GetStringValue(member.getProperty("fipfnr"));


            // school 1
            model.School1.SchoolNaam = ViewModelHelper.GetStringValue(member.getProperty("naamschool_1"));
            // .ToString();
            model.School1.Street = ViewModelHelper.GetStringValue(member.getProperty("straatschool_1"));
            model.School1.Nr = ViewModelHelper.GetStringValue(member.getProperty("nrSchool_1"));
            model.School1.Box = ViewModelHelper.GetStringValue(member.getProperty("busschool_1"));
            model.School1.Zip = ViewModelHelper.GetStringValue(member.getProperty("zipcodeschool_1"));
            model.School1.Location = ViewModelHelper.GetStringValue(member.getProperty("plaatsschool_1"));
            model.School1.Tel = ViewModelHelper.GetStringValue(member.getProperty("telschool_1"));
            model.School1.Email = ViewModelHelper.GetStringValue(member.getProperty("emailschool_1"));


            // school 2
            model.School2.SchoolNaam = ViewModelHelper.GetStringValue(member.getProperty("naamschool_2"));
            model.School2.Street = ViewModelHelper.GetStringValue(member.getProperty("straatschool_2"));
            model.School2.Nr = ViewModelHelper.GetStringValue(member.getProperty("nrschool_2"));
            model.School2.Box = ViewModelHelper.GetStringValue(member.getProperty("busschool_2"));
            model.School2.Zip = ViewModelHelper.GetStringValue(member.getProperty("zipcodeschool_2"));
            model.School2.Location = ViewModelHelper.GetStringValue(member.getProperty("plaatsschool_2"));
            model.School2.Tel = ViewModelHelper.GetStringValue(member.getProperty("telschool_2"));
            model.School2.Email = ViewModelHelper.GetStringValue(member.getProperty("emailschool_2"));

            model.rbGenderOptions.SelectedValue =
                (int) Enum.Parse(typeof (Gender), ViewModelHelper.GetStringValue(member.getProperty("gender")));

            GetMembershipExpirationInfo(member, model);
            return model;
        }

        private static void GetMembershipExpirationInfo(Member member, MemberProfileViewModel model)
        {
            var expirationDate =
                DateTime.Parse(ViewModelHelper.GetStringValue(member.getProperty("subscriptionExpiration")));
            model.MembershipToExpireSoon = expirationDate.AddDays(0 - Settings.SubscriptionExpiry).Ticks <=
                                           DateTime.Now.Ticks;

            var ts = expirationDate.Subtract(DateTime.Now);
            model.DaysToExpiration = ts.Days.ToString();
        }
    }

    public enum BvlfMemberStatus
    {
        Ingeschreven = 14,
        Betaald = 15,
        Actief = 11,
        Slapend = 12,
        Permanent = 13,
        Verwijderd = 17,
        Studiedag = 33
    }

    public enum BvlfMemberProfile
    {
        Student = 1,
        Teacher = 2,
        Retired = 3,
        Other = 4
    }

    public enum Gender
    {
        [Display(Name = "Mr")] Male = 1,

        [Display(Name = "Mevr")] Female = 2
    }

    public enum SubscriptionStatus
    {
        Ingeschreven = 0,
        Betaald = 1
    }
}