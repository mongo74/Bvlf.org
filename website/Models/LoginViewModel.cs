using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace bvlf_v2.Models
{
    /// <summary>
    ///     Summary description for LoginViewModel
    /// </summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Geef je je gebruikersnaam in?")]
        //[RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Dit is geen geldig emailadres")]
        [DisplayName("Gebruikersnaam ")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Vergeet je paswoord niet!")]
        [DisplayName("Paswoord")]
        [RegularExpression(@"^.*(?=.{8,})(?=.*[A-Z])(?=.*[\d\W]).*$",
            ErrorMessage = "Paswoord moet 8 tekens lang zijn, 1 hoofdletter en 1 cijfer bevatten en 1 speciaal teken")]
        public string Password { get; set; }

        [DisplayName("Ingelogd blijven")]
        public bool RememberMe { get; set; }

        public int FailedAttempts { get; set; }
        public string FailedAttemptsMessage { get; set; }
    }
}