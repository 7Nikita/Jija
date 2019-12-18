using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jija.Models;
using Jija.Models.Github;
using System.Linq;
using Jija.Models.Core;
using Microsoft.EntityFrameworkCore;

namespace Jija.Services
{
    public class WebhookService : IWebhookService
    {
        private DatabaseContext _databaseContext;
        private ITicketService _ticketService;

        public WebhookService(DatabaseContext databaseContext, ITicketService ticketService)
        {
            _databaseContext = databaseContext;
            _ticketService = ticketService;
        }
        
        public async Task ProcessPushWebhook(PushWebhookDTO pushWebhookDto)
        {
            var regex = new Regex(@"^refs\/heads\/(.+)$");
            var match = regex.Match(pushWebhookDto.ref_);
            if (!match.Success)
            {
                return;
            }

            var repo = await _databaseContext.Repositories
                .Where(r => r.Id == pushWebhookDto.repository.id)
                .Select(r => r)
                .Include(r => r.Project)
                .SingleOrDefaultAsync();
            
            var ticketName = match.Groups[1].Value;
            var ticket = await _ticketService.FindTicket(repo.Project, ticketName);
            
            if (ticket == null)
            {
                ticket = await _ticketService.CreateTicket(repo.Project, ticketName, 
                    "This ticket was created automatically. Please, adjust.", TicketStatus.Opened);
            }
        }
    }
}
