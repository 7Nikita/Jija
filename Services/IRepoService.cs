using System.Threading.Tasks;
using Jija.Models.Account;
using Jija.Models.Core;

namespace Jija.Services
{
    public interface IRepoService
    {
        Task<bool> CreateRepo(User owner, int id, string name, string description, string htmlUrl);
        Task<bool> DeleteRepo(Repository repository);
        Task<Repository> FindRepo(int id);
    }
}