using Jija.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace JijaTest
{
    public static class Mocks
    {
        
        public static DbContextOptions<DatabaseContext> GetApplicationDbContextOptions()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase("JijaTest")
                .Options;
            return options;
        }
        
        public static IConfigurationRoot GetConfiguration()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            var configuration = configurationBuilder.Build();
            return configuration;
        }
    }
}