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
        public string ApiUrl = "https://api.github.com";
        public string OauthUrl = "https://github.com/login/oauth/access_token";

        public GithubClient(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
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