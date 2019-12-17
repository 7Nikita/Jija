using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

namespace Jija.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet("/Error")]
        public IActionResult Error(int? statusCode = null)
        {
            var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            if (statusCode.HasValue)
            {
                HttpContext.Response.StatusCode = statusCode.Value;
                ViewBag.StatusCode = statusCode.Value;
                ViewBag.RouteOfException = statusCodeData.OriginalPath;
            }

            if (statusCode is 404)
                ViewBag.ErrorMessage = "Sorry the page you requested could not be found";
            else if (statusCode is 500) ViewBag.ErrorMessage = "Sorry something went wrong on the server";

            return View();
        }
    }
}