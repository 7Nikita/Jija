using System.IO;

namespace Jija.Models.Github
{
    public class HttpResponse
    {
        public Stream Response { get; set; }
        public string ErrorMessage { get; set; }

        public HttpResponse(Stream response)
        {
            Response = response;
        }

        public HttpResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}