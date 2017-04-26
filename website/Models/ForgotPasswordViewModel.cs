using System.ComponentModel.DataAnnotations;

namespace bvlf_v2.Models
{
    public class ForgotPasswordViewModel
    {
        public ForgotPasswordViewModel()
        {
            MailSent = false;
        }

        public bool MailSent { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
            ErrorMessage = "Dit is geen geldig emailadres")]
        public string Email { get; set; }
    }
}