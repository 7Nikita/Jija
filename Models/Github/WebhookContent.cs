namespace Jija.Models.Github
{
    public class WebhookContent
    {    
        public class Config
        {
            public string url { get; set; }
            public string content_type { get; set; } = "json";
        }
        
        public Config config { get; set; }
        public string[] events { get; set; }
    }
}