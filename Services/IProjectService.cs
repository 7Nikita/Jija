using System.Threading.Tasks;
using Jija.Models.Account;
using Jija.Models.Core;

namespace Jija.Services
{
    public interface IProjectService
    {
        Task<bool> CreateProject(Project project);
        Task<Project> Find(int id);
        Task<Project> Find(Repository repository);
        Task RemoveContributor(ProjectUser user);
        Task<bool> IsContributor(Project project, User user);
    }
}