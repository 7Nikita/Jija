using System.ComponentModel.DataAnnotations;

namespace Jija.Models.Core
{
    public class InviteForm
    {
        [Required]
        [MaxLength(128, ErrorMessage = "Name max(length) = 128.")]
        public string Username { get; set; }
    }
}