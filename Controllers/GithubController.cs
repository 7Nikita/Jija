using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebHooks;
using Newtonsoft.Json.Linq;

namespace Jija.Controllers
{
    public class GitHubController : ControllerBase
    {
        [Route("/GitHubController/Webhook/{id?}")]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public string Webhook([FromBody] string content)
        {
            return content;
        }
    }
}