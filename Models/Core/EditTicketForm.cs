using System.ComponentModel.DataAnnotations;

namespace Jija.Models.Core
{
    public class EditTicketForm
    {
        [Required]
        [MaxLength(128, ErrorMessage="Name max(length) = 128.")]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        
        [Required] 
        public TicketStatus Status { get; set; }
    }
}