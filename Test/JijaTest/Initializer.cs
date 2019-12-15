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
        private static readonly List<Project> fakeProjects = new List<Project>();
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
                    },
                    new Repository
                    {
                        Id = 7,
                        Name = "Sonic-X"
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
                        Id = "33",
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
            
            fakeProjects.AddRange( new []
                {
                    new Project
                    {
                        Id = 1,
                        Name = "Legend of Zelda",
                        Repository = fakeRepos[0],
                        Contributors = new List<ProjectUser>
                        {
                            new ProjectUser
                            {
                                Contributor =  fakeUsers[0],
                                ContributorId = fakeUsers[0].Id
                            }
                        }
                    }   
                }
            );
        }
        
        public static void InitializeDbForTests(DatabaseContext dbContext)
        {
            dbContext.Users.AddRange(fakeUsers);
            dbContext.Projects.AddRange(fakeProjects);
            dbContext.Repositories.AddRange(fakeRepos);
            dbContext.GithubUsers.AddRange(fakeGithubUsers);
            dbContext.SaveChanges();
        }

    }
}