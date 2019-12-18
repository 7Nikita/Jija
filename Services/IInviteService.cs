using System.Threading.Tasks;
using Jija.Models.Core;

namespace Jija.Services
{
    public interface IInviteService
    {
        Task<bool> CreateInvite(Project project, string username, string msg);
        Task<bool> AcceptInvite(Invite invite);
        Task DeleteInvite(Invite invite);
        Task<bool> RemoveInvite(Invite invite);
    }
}