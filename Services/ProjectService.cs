using System.Linq;
using System.Threading.Tasks;
using Jija.Models;
using Jija.Models.Account;
using Jija.Models.Core;
using Microsoft.EntityFrameworkCore;

namespace Jija.Services
{
    public class ProjectService
    {
        private DatabaseContext _dbContext;

        public ProjectService(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public async Task<bool> CreateProject(Project project)
        {
            var exists = await Find(project.Repository);

            if (exists != null)
            {
                // Todo: Toast
                return false;
            }
            
            await _dbContext.Projects.AddAsync(project);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<Project> Find(int id) =>
            await _dbContext.Projects
                .Include(p => p.Owner)
                .Include(p => p.Repository)
                .Include(p => p.Invites)
                .ThenInclude(inv => inv.User)
                .Include(p => p.Contributors)
                .ThenInclude(contr => contr.Contributor)
                .Include(p => p.Tickets)
                .ThenInclude(ticket => ticket.Assignees)
                .ThenInclude(asg => asg.AssignedUser)
                .SingleOrDefaultAsync();

        public async Task<Project> Find(Repository repository) =>
            await _dbContext.Projects.Where(p => p.Repository.Id == repository.Id).SingleOrDefaultAsync();

        public async Task RemoveContributor(ProjectUser user)
        {
            _dbContext.Remove(user);

            _dbContext.Assignees.RemoveRange(
                await _dbContext.Assignees.Where(asg =>
                        asg.AssigneeId == user.ContributorId && asg.Ticket.ProjectId == user.ProjectId)
                    .ToListAsync()
            );

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsContributor(Project project, User user)
        {
            return await _dbContext.ProjectUsers
                       .Where(u =>
                           u.ProjectId == project.Id &&
                           u.ContributorId == user.Id)
                       .SingleOrDefaultAsync() != null;
        }
    }
}