using System.ComponentModel.DataAnnotations;

namespace Jija.Models.Account
{
    public class RegisterForm
    {
        [Required]
        [MaxLength(128, ErrorMessage = "Too many characters in user name.")]
        public string UserName { get; set; }

        [Required]
        [MaxLength(128, ErrorMessage = "Too many characters in e-mail.")]
        public string Email { get; set; }

        [Required] public string Password { get; set; }
    }
}