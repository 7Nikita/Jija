using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jija.Models.Account;

namespace Jija.Models.Core
{
    public class Project
    {
        public int Id { get; set; }

        [MaxLength(64), Required] public string Name { get; set; }

        public string OwnerId { get; set; }

        public User Owner { get; set; }

        [InverseProperty("Project")] public List<Invite> Invites { get; set; }

        public List<ProjectUser> Contibutors { get; set; }
    }
}