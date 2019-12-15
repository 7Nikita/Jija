using System.Linq;
using Jija.Models;
using Jija.Models.Core;
using Jija.Services;
using Jija.Services.Github;
using Moq;
using NUnit.Framework;

namespace JijaTest.Services
{
    public class ProjectServiceTest
    {
        private readonly DatabaseContext _dbContext = new DatabaseContext(Mocks.GetApplicationDbContextOptions());

        private readonly Mock<IGithubService> _githubService = new Mock<IGithubService>();
        
        [OneTimeSetUp]
        public void SetUp()
        {
            Initializer.InitializeDbForTests(_dbContext);
        }

        [Test]
        public void CreateProjectTest()
        {
            var service = new ProjectService(_dbContext, _githubService.Object);

            var fakeRepo = _dbContext.Repositories.FirstOrDefault(r => r.Id == 7);
            
            var res = service.CreateProject(new Project {Id = 888, Name = "FakeProject", Repository = fakeRepo});

            var project = _dbContext.Projects.FirstOrDefault(p => p.Id == 888);
            
            Assert.IsTrue(res.Result);
            Assert.AreEqual(888, project.Id);
            Assert.AreEqual("FakeProject", project.Name);
        }

        [Test]
        public void FindByIdTest()
        {
            var service = new ProjectService(_dbContext, _githubService.Object);

            var project = service.Find(1);

            Assert.AreEqual(1, project.Result.Id);
            Assert.AreEqual("Legend of Zelda", project.Result.Name);
            Assert.AreEqual("MarioKart", project.Result.Repository.Name);
        }

        [Test]
        public void FindByRepositoryTest()
        {
            var service = new ProjectService(_dbContext, _githubService.Object);

            var repository = _dbContext.Repositories.FirstOrDefault(r => r.Id == 1);

            var project = service.Find(repository);
            
            Assert.AreEqual(1, project.Result.Id);
            Assert.AreEqual("Legend of Zelda", project.Result.Name);
        }
        
        [Test]
        public void IsContributorTest()
        {
            var service = new ProjectService(_dbContext, _githubService.Object);

            var project = _dbContext.Repositories.FirstOrDefault(p => p.Id == 1);
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == "1");

            var res = service.IsContributor(project.Project, user);
            
            Assert.IsTrue(res.Result);
        }

        [Test]
        public void IsNotContributorTest()
        {
            var servive = new ProjectService(_dbContext, _githubService.Object);

            var project = _dbContext.Repositories.FirstOrDefault(p => p.Id == 1);
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == "33");

            var res = servive.IsContributor(project.Project, user);
            
            Assert.IsFalse(res.Result);
        }
        
        [OneTimeTearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}