using System.ComponentModel.DataAnnotations;
using Jija.Models.Account;

namespace Jija.Models.Core
{
    public class Repository
    {
        public int Id { get; set; }

        [MaxLength(64), Required] public string Name { get; set; }

        public string UserId { get; set; }

        public string Description { get; set; }
        
        public string HtmlUrl { get; set; }
        
        public User Owner { get; set; }
        
        public int? ProjectId { get; set; }
        
        public Project Project { get; set; }
    }
}