using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Jija.Models.Github;
using Microsoft.Extensions.Configuration;

namespace Jija.Services.Github
{
    public class GithubClient : IGithubClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        
        public string Token { get; set; }
        public string AzureUrl;
        public string ApiUrl;
        public string OauthUrl;

        public GithubClient(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;

            AzureUrl = _configuration["Routes:AzureURL"];
            ApiUrl = _configuration["Routes:ApiUrl"];
            OauthUrl = _configuration["Routes:OauthUrl"];
            
        }

        public async Task<HttpResponse> MakeRequest(HttpRequestMessage requestMessage)
        {
            try
            {
                var response = await _httpClient.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode) return new HttpResponse(responseContent);

                var failedRequestDTO = await JsonSerializer.DeserializeAsync<FailedRequestDTO>(responseContent);
                return new HttpResponse(failedRequestDTO.message);
            }
            catch (HttpRequestException e)
            {
                return new HttpResponse(e.Message);
            }
        }

        public async Task<OauthTokenDTO> GetOauthToken(string code)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, OauthUrl)
            {
                Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("client_id", _configuration["Github:ClientId"]),
                    new KeyValuePair<string, string>("client_secret", _configuration["Github:ClientSecret"])
                })
            };

            request.Headers.Add("Accept", "application/json");

            var response = await MakeRequest(request);

            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                throw new HttpRequestException(response.ErrorMessage);
            }

            return await JsonSerializer.DeserializeAsync<OauthTokenDTO>(response.Response);
        }

        public async Task<UserInfoDTO> GetUserInfo()
        {
            var request = CreateRequestTo($"{ApiUrl}/user");
            var response = await MakeRequest(request);
            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                throw new HttpRequestException(response.ErrorMessage);
            }

            return await JsonSerializer.DeserializeAsync<UserInfoDTO>(response.Response);
        }

        public async Task<List<RepositoryInfoDTO>> GetRepos()
        {
            var request = CreateRequestTo($"{ApiUrl}/user/repos");
            var response = await MakeRequest(request);
            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                throw new HttpRequestException(response.ErrorMessage);
            }

            return await JsonSerializer.DeserializeAsync<List<RepositoryInfoDTO>>(response.Response);
        }

        public async Task<WebhookDTO> CreateWebhook(string owner, string repoName)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, 
                $"{ApiUrl}/repos/{owner}/{repoName}/hooks");
            var content = new WebhookContent()
            {
                config = new WebhookContent.Config
                {
                    url = $"{AzureUrl}/GitHub/Push",
                },
                events = new [] {"push"},
            };
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var rawContent = new StringContent(JsonSerializer.Serialize(content, options));
            request.Content = rawContent;

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", $"token {Token}");
            request.Headers.Add("User-Agent", "Jija");

            var response = await MakeRequest(request);
            if(response.ErrorMessage != null)
            {
                throw new HttpRequestException(response.ErrorMessage);
            }
            var createWebhookResponse = await JsonSerializer.DeserializeAsync<WebhookDTO>(response.Response);       
            return createWebhookResponse;      
        }
        
        private HttpRequestMessage CreateRequestTo(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", $"token {Token}");
            request.Headers.Add("User-Agent", "Jija");

            return request;
        }

    }
}