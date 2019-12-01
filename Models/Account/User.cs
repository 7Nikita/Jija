using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Jija.Models.Core;
using Jija.Models.Github;
using Microsoft.AspNetCore.Identity;

namespace Jija.Models.Account
{
    public class User : IdentityUser
    {
        public User() : base()
        {
        }

        [InverseProperty("Owner")] public List<Project> OwnedProjects { get; set; }

        public List<Invite> Invites { get; set; }

        public List<Repository> Repositories { get; set; }

        public List<ProjectUser> ContributedProjects { get; set; }
        
        public GithubUser GithubUser { get; set; }

    }
}