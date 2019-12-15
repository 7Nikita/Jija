namespace Jija.Models.Github
{
    public class WebhookDTO
    {
        public int id { get; set; }
        
        public string type { get; set; }
        
        public string name { get; set; }
        
        public ConfigDTO config { get; set; }
    }
}