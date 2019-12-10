using System.Collections.Generic;
using System.Threading.Tasks;
using Jija.Models.Account;
using Jija.Models.Github;

namespace Jija.Services.Github
{
    public interface IGithubService
    {
        public string GetOauthRequestUrl();
        Task<ResultDTO<OauthTokenDTO>> SetOauthToken(string code, User user);
        Task<ResultDTO<UserInfoDTO>> SetGithubUserInfo(User user);
        Task<ResultDTO<List<RepositoryInfoDTO>>> GetUserRepositories(User user);
    }
}