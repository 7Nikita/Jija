using Jija.Models.Account;
using Jija.Models.Core;
using Jija.Models.Github;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Jija.Models
{
    public class DatabaseContext : IdentityDbContext<User, Role, string>
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<Invite> Invites { get; set; }
        public DbSet<GithubUser> GithubUsers { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Assignee> Assignees { get; set; }
        
        public DbSet<Webhook> Webhooks { get; set; }


        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProjectUser>().HasKey(
                projectUser => new {projectUser.ContributorId, projectUser.ProjectId}
            );

            builder.Entity<ProjectUser>()
                .HasOne(projectUser => projectUser.Contributor)
                .WithMany(contributor => contributor.ContributedProjects)
                .HasForeignKey(projectUser => projectUser.ContributorId);

            builder.Entity<ProjectUser>()
                .HasOne(projectUser => projectUser.Project)
                .WithMany(project => project.Contributors)
                .HasForeignKey(projectUser => projectUser.ProjectId);

            builder.Entity<Assignee>().HasKey(asg => new {asg.AssigneeId, asg.TicketId});

            builder.Entity<Assignee>()
                .HasOne(asg => asg.AssignedUser)
                .WithMany(asg => asg.Tickets)
                .HasForeignKey(t => t.AssigneeId);
            
            builder.Entity<Assignee>()
                .HasOne(asg => asg.Ticket)
                .WithMany(ticket => ticket.Assignees)
                .HasForeignKey(asg => asg.TicketId);
        }
    }
}