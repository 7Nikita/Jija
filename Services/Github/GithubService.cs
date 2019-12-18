using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Jija.Models;
using Jija.Models.Account;
using Jija.Models.Core;
using Jija.Models.Github;
using Microsoft.Extensions.Configuration;

namespace Jija.Services.Github
{
    public class GithubService : IGithubService
    {
        private IGithubClient _client;
        private DatabaseContext _dbContext;

        private readonly string _oauthUrl;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly IConfiguration _configuration;

        private const string OauthRequestUrl = "https://github.com/login/oauth/" ;

        public GithubService(IConfiguration configuration, IGithubClient client, DatabaseContext databaseContext)
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
        
        public async Task<ResultDTO<OauthTokenDTO>> SetOauthToken(string code, User user)
        {
            try
            {
                var response = await _client.GetOauthToken(code);
                
                user.GithubUser.AccessToken = response.access_token;
                
                await _dbContext.SaveChangesAsync();
                
                return new ResultDTO<OauthTokenDTO>(response);
            }
            catch (HttpRequestException e)
            {
                return new ResultDTO<OauthTokenDTO>(e.Message);
            }
        }

        public async Task<ResultDTO<UserInfoDTO>> SetGithubUserInfo(User user)
        {
            
            try
            {
                _client.Token = user.GithubUser.AccessToken;

                var response = await _client.GetUserInfo();

                user.GithubUser.Login = response.login;
                user.GithubUser.HtmlUrl = response.html_url;
                user.GithubUser.AvatarUrl = response.avatar_url;

                await _dbContext.SaveChangesAsync();
                return new ResultDTO<UserInfoDTO>(response);
            }
            catch (HttpRequestException e)
            {
                return new ResultDTO<UserInfoDTO>(e.Message);
            }
        }

        public async Task<ResultDTO<List<RepositoryInfoDTO>>> GetUserRepositories(User user)
        {
            try
            {
                _client.Token = user.GithubUser.AccessToken;
                var resp = await _client.GetRepos();
                return new ResultDTO<List<RepositoryInfoDTO>>(await _client.GetRepos());
            }
            catch (HttpRequestException e)
            {
                return new ResultDTO<List<RepositoryInfoDTO>>(e.Message);
            }
        }

        public async Task<ResultDTO<WebhookDTO>> CreateWebhook(Project project)
        {
            try
            {
                _client.Token = project.Owner.GithubUser.AccessToken;
                var response = await _client.CreateWebhook(project.Owner.GithubUser.Login, project.Repository.Name);
                return new ResultDTO<WebhookDTO>(response);
            }
            catch (HttpRequestException e)
            {
                return new ResultDTO<WebhookDTO>(e.Message);
            }
        }
    }
}