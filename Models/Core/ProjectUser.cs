using System.ComponentModel.DataAnnotations;

namespace Jija.Models 
{
    public class ProjectUser 
    {
        public string ContributorId { get; set; }

        public User Contributor { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }
    }

}