using System.Linq;
using System.Threading.Tasks;
using Jija.Models;
using Jija.Models.Account;
using Jija.Models.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Jija.Services
{
    public class InviteService : IInviteService
    {
        private IMailing _smtpService;
        private DatabaseContext _dbContext;
        private UserManager<User> _userManager;

        public InviteService(DatabaseContext dbContext, UserManager<User> userManager, IMailing smtpService)
        {
            _dbContext = dbContext;
            _smtpService = smtpService;
            _userManager = userManager;
        }

        public async Task<bool> CreateInvite(Project project, string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return false;

            var inviteExists = await _dbContext.Invites
                .Where(i => i.UserId == user.Id && i.ProjectId == project.Id)
                .SingleOrDefaultAsync();

            var userExists = await _dbContext.ProjectUsers
                .Where(u => u.ContributorId == user.Id && u.ProjectId == project.Id)
                .SingleOrDefaultAsync();

            if (inviteExists != null || userExists != null)
                return false;

            var inv = new Invite
            {
                Project = project,
                UserId = user.Id,
                Status = InviteStatus.Pending
            };

            await _dbContext.Invites.AddAsync(inv);
            await _dbContext.SaveChangesAsync();
            
            // TODO: Send email

            return true;
        }

        public async Task<bool> AcceptInvite(Invite invite)
        {
            var projectUser = new ProjectUser
            {
                ContributorId = invite.UserId,
                ProjectId = invite.ProjectId
            };

            await _dbContext.ProjectUsers.AddAsync(projectUser);
            await DeleteInvite(invite);

            return true;
        }

        public async Task DeleteInvite(Invite invite)
        {
            _dbContext.Invites.Remove(invite);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> RemoveInvite(Invite invite)
        {
            await DeleteInvite(invite);
            return true;
        }
    }
}