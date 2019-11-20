using Jija.Models.Core;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jija.Models 
{
    public class User : IdentityUser 
    {
        
        public User() : base() {}

        [InverseProperty("Owner")]
        public List<Project> OwnedProjects { get; set; }

        public List<Invite> Invites { get; set; }

        public List<Repository> Repositories { get; set; }

        public List<ProjectUser> ContributedProjects { get; set; }

    }
}