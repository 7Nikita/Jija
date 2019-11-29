using System;
using System.Net.Http;
using System.Threading.Tasks;
using Jija.Models;
using Jija.Models.Github;
using Microsoft.Extensions.Configuration;

namespace Jija.Services.Github
{
    public class GithubService
    {
        private GithubClient _client;
        private DatabaseContext _dbContext;

        private readonly string _oauthUrl;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly IConfiguration _configuration;

        private const string OauthRequestUrl = "https://github.com/login/oauth/" ;

        public GithubService(IConfiguration configuration, GithubClient client, DatabaseContext databaseContext)
        {
            _client = client;
            _dbContext = databaseContext;
            _configuration = configuration;

            _clientId = _configuration["Github:ClientId"];
            _clientSecret = _configuration["Github:ClientSecret"];

            _oauthUrl =
                $"{OauthRequestUrl}authorize?client_id={_clientId}&client_secret={_clientSecret}&scope=user,repo";
        }

        public string GetOauthRequestUrl() => _oauthUrl;
        
        public async Task<OauthTokenResultDTO> SetOauthToken(string code, User user)
        {
            try
            {
                var response = await _client.GetOauthToken(code);
                
                user.GithubUser.AccessToken = response.access_token;
                
                await _dbContext.SaveChangesAsync();
                
                return new OauthTokenResultDTO(response);
            }
            catch (HttpRequestException e)
            {
                return new OauthTokenResultDTO(e.Message);
            }
        }

        public async Task<UserInfoResultDTO> SetGithubUserInfo(User user)
        {
            
            try
            {
                _client.Token = user.GithubUser.AccessToken;

                var response = await _client.GetUserInfo();

                user.GithubUser.Login = response.login;
                user.GithubUser.HtmlUrl = response.html_url;
                user.GithubUser.AvatarUrl = response.avatar_url;

                await _dbContext.SaveChangesAsync();
                return new UserInfoResultDTO(response);
            }
            catch (HttpRequestException e)
            {
                return new UserInfoResultDTO(e.Message);
            }
        }
    }
}