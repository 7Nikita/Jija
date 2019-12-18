using System.Threading.Tasks;
using Jija.Models;
using Jija.Models.Github;

namespace Jija.Services
{
    public interface IWebhookService
    {
        Task ProcessPushWebhook(PushWebhookDTO pushWebhookDto);
    }
}