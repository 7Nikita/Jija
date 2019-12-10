using System.Collections.Generic;
using Jija.Models;
using Jija.Models.Account;
using Jija.Models.Core;
using Jija.Models.Github;

namespace JijaTest
{
    public static class Initializer
    {
        private static readonly List<User> fakeUsers = new List<User>();
        private static readonly List<Repository> fakeRepos = new List<Repository>();
        private static readonly List<GithubUser> fakeGithubUsers = new List<GithubUser>();

        static Initializer()
        {
            fakeRepos.AddRange(new []
                {
                    new Repository
                    {
                        Id = 1,
                        Name = "MarioKart"
                    }         
                }
            );
            
            fakeUsers.AddRange(new[]
                {
                    new User
                    {
                        Id = "1",
                        UserName = "7Nikita",
                        Email = "test@mail.ru",
                        Repositories = fakeRepos
                    },
                    new User
                    {
                        UserName = "Mario",
                        Email = "mario@luigi.com",
                        GithubUser = new GithubUser
                        {
                            Id = 123,
                            AccessToken = "123"
                        }
                    }
                }
            );
            
            fakeGithubUsers.AddRange(new []
            {
                new GithubUser
                {
                    Login = "Guido",
                    HtmlUrl = "https://github.com/guido",
                    AvatarUrl = "https://frankerfaces.com"
                }
            });
        }
        
        public static void InitializeDbForTests(DatabaseContext dbContext)
        {
            dbContext.Users.AddRange(fakeUsers);
            dbContext.Repositories.AddRange(fakeRepos);
            dbContext.GithubUsers.AddRange(fakeGithubUsers);
            dbContext.SaveChanges();
        }

    }
}