using System.Collections.Generic;

namespace Jija.Models.Github
{
    public class RepositoryInfoDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string html_url { get; set; }
        public string description { get; set; }
    }
}