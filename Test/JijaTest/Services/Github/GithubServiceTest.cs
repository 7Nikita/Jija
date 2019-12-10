using System.Collections.Generic;
using System.Linq;
using Jija.Models;
using Jija.Models.Account;
using Jija.Models.Github;
using Jija.Services.Github;
using Moq;
using NUnit.Framework;

namespace JijaTest.Services.Github
{
    public class GithubServiceTest
    {
        private readonly DatabaseContext _dbContext = new DatabaseContext(Mocks.GetApplicationDbContextOptions());

        private readonly Mock<IGithubClient> _githubClient = new Mock<IGithubClient>();
        
        [SetUp]
        public void SetUp()
        {
            Initializer.InitializeDbForTests(_dbContext);
        }

        [Test]
        public void GetUserRepositoriesTest()
        {
            var repos = new List<RepositoryInfoDTO>();
            repos.Add(new RepositoryInfoDTO {id = 1, name = "MicrosoftTests"});

            _githubClient.Setup(req => req.GetRepos()).ReturnsAsync(repos);
            var service = new GithubService(Mocks.GetConfiguration(), _githubClient.Object, _dbContext);
            var resp = service.GetUserRepositories(new User{ GithubUser = new GithubUser {AccessToken = "123"}});

            Assert.AreEqual(1, resp.Result.Response.Count);
            Assert.AreEqual("MicrosoftTests", resp.Result.Response[0].name);
        }

        [Test]
        public void SetGithubUserInfo()
        {
            _githubClient.Setup(req => req.GetUserInfo()).ReturnsAsync(new UserInfoDTO
            {
                login = "Guido",
                html_url = "https://github.com/guido",
                avatar_url = "https://frankerfaces.com"
            });
            
            var fakeUser = _dbContext.Users.FirstOrDefault(user => user.UserName == "Mario");
            
            var service = new GithubService(Mocks.GetConfiguration(), _githubClient.Object, _dbContext);

            var resp = service.SetGithubUserInfo(fakeUser);

            Assert.AreEqual("Guido", resp.Result.Response.login);
            
            var dbUser = _dbContext.Users.FirstOrDefault(user => user.UserName == "Mario");
            Assert.AreEqual("https://github.com/guido", dbUser.GithubUser.HtmlUrl);
        }
        
        [TearDown]
        public void TearDown()
        { 
            _dbContext.Database.EnsureDeleted();
        }
        
    }
}