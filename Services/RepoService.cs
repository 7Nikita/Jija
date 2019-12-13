using System.Linq;
using System.Threading.Tasks;
using Jija.Models;
using Jija.Models.Account;
using Jija.Models.Core;
using Microsoft.EntityFrameworkCore;

namespace Jija.Services
{
    public class RepoService
    {
        private DatabaseContext _dbContext;
        private ProjectService _projectService;

        public RepoService(DatabaseContext databaseContext, ProjectService projectService)
        {
            _dbContext = databaseContext;
            _projectService = projectService;
        }

        public async Task<bool> CreateRepo(User owner, int id, string name, string description, string htmlUrl)
        {
            if (_dbContext.Repositories.SingleOrDefault(r => r.Id == id && r.Owner.Id == owner.Id) != null)
            {
                // Todo: Toast
                return false;
            }

            var repo = new Repository
            {
                Owner = owner,
                Id = id,
                Name = name,
                Description = description,
                HtmlUrl = htmlUrl,
                ProjectId = null
            };

            await _dbContext.Repositories.AddAsync(repo);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRepo(Repository repository)
        {
            var exists = await _projectService.Find(repository);
            if (exists != null)
            {
                // Todo: Toast
                return false;
            }

            _dbContext.Repositories.Remove(repository);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Repository> FindRepo(int id) =>
            await _dbContext.Repositories.Where(repo => repo.Id == id).SingleOrDefaultAsync();
    }
}