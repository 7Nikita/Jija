using System.ComponentModel.DataAnnotations;

namespace Jija.Models.Account
{
    public class EditAccountForm
    {
        [Required]
        [MaxLength(128, ErrorMessage = "Username max(length) = 128")]
        public string Username { get; set; }
        
        [Required]
        [MaxLength(128, ErrorMessage = "Emai; max(length) = 128")]
        public string Email { get; set; }
    }
}