using Jija.Models.Account;

namespace Jija.Models.Core
{
    public class Assignee
    {
        public string AssigneeId { get; set; }
        
        public User AssignedUser { get; set; }
        
        public int TicketId { get; set; }
        
        public Ticket Ticket { get; set; }
    }
}