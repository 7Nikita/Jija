using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Jija.Models.Github;

namespace Jija.Services.Github
{
    public interface IGithubClient
    {
        public string Token { get; set; }
        Task<HttpResponse> MakeRequest(HttpRequestMessage requestMessage);
        Task<OauthTokenDTO> GetOauthToken(string code);
        Task<UserInfoDTO> GetUserInfo();
        Task<List<RepositoryInfoDTO>> GetRepos();
        Task<WebhookDTO> CreateWebhook(string owner, string repoName);
    }
}