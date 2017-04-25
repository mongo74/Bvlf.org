using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace bvlf_v2.Models
{
    public class ChangePasswordViewModel
    {
        public ChangePasswordViewModel()
        {
            PasswordHasChanged = false;
        }

        public bool PasswordHasChanged { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Nieuw paswoord")]
        [RegularExpression(@"^.*(?=.{8,})(?=.*[A-Z])(?=.*[\d\W]).*$",
            ErrorMessage = "Paswoord moet 8 tekens lang zijn, 1 hoofdletter en 1 cijfer bevatten en 1 speciaal teken")]
        public string Password { get; set; }

        [Required(ErrorMessage = "*")]
        [DisplayName("Bevestig nieuw paswoord")]
        [RegularExpression(@"^.*(?=.{8,})(?=.*[A-Z])(?=.*[\d\W]).*$",
            ErrorMessage = "Paswoord moet 8 tekens lang zijn, 1 hoofdletter en 1 cijfer bevatten en 1 speciaal teken")]
        public string ComparePassword { get; set; }
    }
}