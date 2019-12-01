using System.Collections.Generic;

namespace Jija.Models.Github
{
    public class RepositoryInfoDTO
    {
        public long id { get; set; }
        public string name { get; set; }
        public string html_url { get; set; }
        public UserInfoDTO owner { get; set; }
    }
}