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
                .WithMany(project => project.Contibutors)
                .HasForeignKey(projectUser => projectUser.ProjectId);
        }
    }
}