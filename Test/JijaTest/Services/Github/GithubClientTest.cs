using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Jija.Services.Github;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace JijaTest.Services.Github
{
    public class GithubClientTest
    {
        private GithubClient _client;
        private FluentMockServer _server;
        private IConfigurationRoot _config;
        private HttpClient _httpClient = new HttpClient();

        [SetUp]
        public void Setup()
        {
            var configDict = new Dictionary<string, string>
            {
                {"code", "123"},
                {"client_id", "63978fd92a5161b022a0"},
                {"client_secret", "d1b8932df78a653813d2fe5e67a4f0a63943c4ea"}
            };
            _config = new ConfigurationBuilder().AddInMemoryCollection(configDict).Build();

            _server = FluentMockServer.Start();
        }

        [Test]
        public async Task TestGetOauthToken()
        {
            const string oauthResp = "{ \"access_token\": \"31ccdad924fb247281124be8691955a85054d833\" }";

            _server.Given(Request.Create()
                    .WithPath("/")
                    .UsingPost()
                )
                .RespondWith(Response.Create()
                    .WithStatusCode(250)
                    .WithBody(oauthResp)
                );

            _client = new GithubClient(_config, _httpClient) {OauthUrl = _server.Urls[0]};

            var oauthToken = await _client.GetOauthToken("sampleToken");

            Assert.AreEqual("31ccdad924fb247281124be8691955a85054d833", oauthToken.access_token);
        }

        [Test]
        public async Task TestGetUserInfo()
        {
            const string userResp = "{" +
                                    "\"login\": \"7Nikita\"," +
                                    "\"avatar_url\": \"https://avatars1.githubusercontent.com/u/20500960?v=4\"," +
                                    "\"html_url\": \"https://github.com/7Nikita\"," +
                                    "\"bio\": null" +
                                    "}";

            _server.Given(Request.Create()
                    .WithPath("/user")
                    .UsingGet()
                )
                .RespondWith(Response.Create()
                    .WithStatusCode(250)
                    .WithBody(userResp)
                );

            _client = new GithubClient(_config, _httpClient) {Token = "testToken", ApiUrl = _server.Urls[0]};
            var userInfo = await _client.GetUserInfo();

            Assert.AreEqual("7Nikita", userInfo.login);
            Assert.AreEqual("https://github.com/7Nikita", userInfo.html_url);
        }

        [Test]
        public async Task TestGetRepos()
        {
            const string reposResp = "[{\"id\": 221949625," +
                                     "\"name\": \"Jija\"," +
                                     "\"html_url\": \"https://github.com/7Nikita/Jija\"," +
                                     "\"owner\": {" +
                                     "\"login\": \"7Nikita\"," +
                                     "\"avatar_url\": \"https://avatars1.githubusercontent.com/u/20500960?v=4\"," +
                                     "\"html_url\": \"https://github.com/7Nikita\"" +
                                     "}}" +
                                     "]";
            
            _server.Given(Request.Create()
                    .WithPath("/user/repos")
                    .UsingGet()
                )
                .RespondWith(Response.Create()
                    .WithStatusCode(250)
                    .WithBody(reposResp)
                );
            
            _client = new GithubClient(_config, _httpClient) {Token = "testToken", ApiUrl = _server.Urls[0]};

            var reposInfo = await _client.GetRepos();
            
            Assert.AreEqual(1, reposInfo.Count);
            Assert.AreEqual("7Nikita", reposInfo[0].owner.login);
        }
    }
}