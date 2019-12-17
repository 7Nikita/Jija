using System.ComponentModel.DataAnnotations;

namespace Jija.Models.Core
{
    public class NewProjectForm
    {
        [Required]
        [MaxLength(128, ErrorMessage="Name max(length) = 128.")]
        public string Name {get; set;}
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage="No repository selected.")]
        public int RepositoryId {get; set;}
    }
}