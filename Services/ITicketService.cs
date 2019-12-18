using System.Collections.Generic;
using System.Threading.Tasks;
using Jija.Models.Account;
using Jija.Models.Core;

namespace Jija.Services
{
    public interface ITicketService
    {
        Task<Ticket> CreateTicket(Project project, string name, string desc, TicketStatus status);
        Task<List<Ticket>> FindTicketByStatus(Project project, TicketStatus status);
        Task<Ticket> FindTicket(Project project, string name);
        Task<Ticket> FindTicket(Project project, int id);
        Task<bool> UpdateTicket(Ticket ticket, string name, string desc, TicketStatus status);
        Task<bool> DeleteTicket(Ticket ticket);
        Task<bool> AddUser(Ticket ticket, User user);
        Task<bool> RemoveUser(Ticket ticket, User user);
    }
}