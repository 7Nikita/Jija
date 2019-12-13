using System.Linq;
using System.Threading.Tasks;
using Jija.Models;
using Jija.Models.Account;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.EntityFrameworkCore;

namespace Jija.Services
{
    public class DbService : ServerAuthenticationStateProvider
    {
        private DatabaseContext _dbContext;
        
        public DbService(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }
        public void NotifyStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<User> GetUser()
        {
            var authState = await GetAuthenticationStateAsync();
            return await _dbContext.Users
                .Include(user => user.GithubUser)
                .Where(user => user.UserName == authState.User.Identity.Name)
                .SingleOrDefaultAsync();
        }

        public async Task<User> GetUserAndProjects()
        {
            var authState = await GetAuthenticationStateAsync();
            return await _dbContext.Users
                .Include(user => user.GithubUser)
                .Include(user => user.OwnedProjects)
                .Include(user => user.ContributedProjects)
                .ThenInclude(cProjects => cProjects.Project)
                .Where(user => user.UserName == authState.User.Identity.Name)
                .SingleOrDefaultAsync();
        }

        public async Task<User> GetUserAndRepos()
        {
            var authState = await GetAuthenticationStateAsync();
            return await _dbContext.Users
                .Include(user => user.GithubUser)
                .Include(user => user.Repositories)
                .Where(user => user.UserName == authState.User.Identity.Name)
                .SingleOrDefaultAsync();
        }
        
        public async Task<User> GetAll()
        {
            var authState = await GetAuthenticationStateAsync();
            return await _dbContext.Users
                .Include(user => user.GithubUser)
                .Include(user => user.OwnedProjects)
                .Include(user => user.Invites)
                .ThenInclude(invite => invite.Project)
                .ThenInclude(project => project.Repository)
                .Include(user => user.Repositories)
                .Include(user => user.ContributedProjects)
                .ThenInclude(cProjects => cProjects.Project)
                .ThenInclude(project => project.Repository)
                .Where(user => user.UserName == authState.User.Identity.Name)
                .SingleOrDefaultAsync();
        }
        
    }
}