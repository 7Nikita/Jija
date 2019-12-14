using System.ComponentModel.DataAnnotations;

namespace Jija.Models.Account
{
    public class EditPassForm
    {
        [Required]
        public string OldPass { get; set; }
        
        [Required]
        public string NewPass { get; set; }
}
}