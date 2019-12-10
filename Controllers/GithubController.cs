using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebHooks;
using Newtonsoft.Json.Linq;

namespace Jija.Controllers
{
    public class GithubController : ControllerBase
    {
        [GitHubWebHook]
        public IActionResult Github(string id, JObject data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }
        
    }
}