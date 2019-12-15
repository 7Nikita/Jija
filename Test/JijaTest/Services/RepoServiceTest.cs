using System.Linq;
using Jija.Models;
using Jija.Services;
using Moq;
using NUnit.Framework;


namespace JijaTest.Services
{
    public class RepoServiceTest
    {
        private readonly DatabaseContext _dbContext = new DatabaseContext(Mocks.GetApplicationDbContextOptions());

        private readonly Mock<IProjectService> _projectService = new Mock<IProjectService>();
        
        [OneTimeSetUp]
        public void SetUp()
        {
            Initializer.InitializeDbForTests(_dbContext);
        }

        [Test]
        public void CreateRepoTest()
        {
            var service = new RepoService(_dbContext, _projectService.Object);
            var fakeUser = _dbContext.Users.FirstOrDefault(user => user.UserName == "Mario");

            service.CreateRepo(fakeUser, 999, "RepoFromHell", "Test repo", "http://2ip.ru");

            var repo = service.FindRepo(999);
            
            Assert.AreEqual("RepoFromHell", repo.Result.Name);
            Assert.AreEqual("Test repo", repo.Result.Description);
        }
        
        [Test]
        public void FindRepoTest()
        {
            var service = new RepoService(_dbContext, _projectService.Object);

            var repo = service.FindRepo(1);

            Assert.AreEqual(1, repo.Result.Id);
            Assert.AreEqual("MarioKart", repo.Result.Name);
        }

        [Test]
        public void DeleteRepoTest()
        {
            var service = new RepoService(_dbContext, _projectService.Object);
            var fakeUser = _dbContext.Users.FirstOrDefault(user => user.UserName == "Mario");

            service.CreateRepo(fakeUser, 998, "RepoToDelete", "Test repo", "http://dev.to");
            var res = service.DeleteRepo(_dbContext.Repositories.FirstOrDefault(repo => repo.Id == 998));
            
            Assert.AreEqual(true, res.Result);
        }
        
        [OneTimeTearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}