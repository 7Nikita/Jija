using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebHooks;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Jija.Models.Github;
using Jija.Services;

namespace Jija.Controllers
{
    public class GitHubController : Controller
    {
        private IWebhookService _webhookService;
        public GitHubController(IWebhookService webhookService)
        {
            _webhookService = webhookService;
        }
        
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Push([FromBody] PushWebhookDTO pushWebhookDto)
        {
            await _webhookService.ProcessPushWebhook(pushWebhookDto);
            return Ok();
        }
    }
}