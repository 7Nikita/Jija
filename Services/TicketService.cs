using System.Collections.Generic;
using Jija.Models;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Jija.Models.Account;
using Jija.Models.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Jija.Services
{
    public class TicketService : ITicketService
    {
        private IMailing _smtpService;
        private DatabaseContext _dbContext;
        private UserManager<User> _userManager;

        public TicketService(DatabaseContext dbContext, UserManager<User> userManager, IMailing smtpService)
        {
            _dbContext = dbContext;
            _smtpService = smtpService;
            _userManager = userManager;
        }

        public async Task<Ticket> CreateTicket(Project project, string name, string desc, TicketStatus status)
        {
            var exists = await FindTicket(project, name);

            if (exists != null)
                return null;

            var ticket = new Ticket
            {
                ProjectId = project.Id,
                Name = name,
                Description = desc,
                Status = status
            };

            await _dbContext.Tickets.AddAsync(ticket);
            await _dbContext.SaveChangesAsync();

            return ticket;
        }

        public async Task<Ticket> FindTicket(Project project, string name) =>
            await _dbContext.Tickets
                .Where(t => t.ProjectId == project.Id && t.Name == name)
                .SingleOrDefaultAsync();

        public async Task<List<Ticket>> FindTicketByStatus(Project project, TicketStatus status) =>
            await _dbContext.Tickets
                .Where(t => t.ProjectId == project.Id && t.Status == status)
                .ToListAsync();

        public async Task<Ticket> FindTicket(Project project, int id) =>
            await _dbContext.Tickets
                .Where(t => t.ProjectId == project.Id && t.Id == id)
                .SingleOrDefaultAsync();

        public async Task<bool> UpdateTicket(Ticket ticket, string name, string desc, TicketStatus status)
        {
            ticket.Name = name;
            ticket.Description = desc;
            ticket.Status = status;

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTicket(Ticket ticket)
        {
            _dbContext.Tickets.Remove(ticket);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddUser(Ticket ticket, User user)
        {
            var asg = await _dbContext.Assignees
                .Where(a => a.Ticket.Id == ticket.Id && a.AssigneeId == user.Id)
                .SingleOrDefaultAsync();

            if (asg != null)
                return false;

            await _dbContext.Assignees.AddAsync(new Assignee
            {
                TicketId = ticket.Id,
                AssigneeId = user.Id
            });

            await _dbContext.SaveChangesAsync();

            return true;
        }
        
        public async Task<bool> RemoveUser(Ticket ticket, User user)
        {
            var asg = await _dbContext.Assignees
                .Where(a => a.Ticket.Id == ticket.Id && a.AssigneeId == user.Id)
                .SingleOrDefaultAsync();

            if (asg != null)
                return false;

            _dbContext.Assignees.Remove(asg);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        
    }
}