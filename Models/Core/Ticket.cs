using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jija.Models.Core
{
    public enum TicketStatus
    {
        Opened,
        InProgress,
        OnReview,
        Done
    }
    
    public class Ticket
    {
        public int Id { get; set; }
        
        [MaxLength(128), Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        public List<Assignee> Assignees { get; set; }
        
        public TicketStatus Status { get; set; }
        
        public int ProjectId { get; set; }
        
        public Project Project { get; set; }
    }
}