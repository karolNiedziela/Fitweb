using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.CommandQueryHandler.Commands.Athletes;
using Backend.Infrastructure.EF;
using Backend.Tests.Integration.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Backend.Tests.Integration.Controllers
{
    public class AthletesControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public AthletesControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Get_ShouldReturnOk_WhenAthleteExists()
        {
            var client = FreshClient();

            var signUp = new SignUp
            {
                Id = 3,
                Username = "testAthlete",
                Email = "testAthlete@email.com",
                Password = "testPassword"
            };

            await client.PostAsJsonAsync($"/api/account/signup", signUp);

            await client.PostAsJsonAsync($"/api/athletes", new CreateAthlete
            {
                UserId = signUp.Id
            });

            var response = await client.GetAsync($"/api/athletes/{signUp.Id}");

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_ShouldReturnNotFound_WhenAthleteDoesNotExist()
        {
            var client = FreshClient();
            var userId = 3;

            var response = await client.GetAsync($"/api/athletes/{userId}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk()
        {
            var client = FreshClient();

            var signUp = new SignUp
            {
                Id = 3,
                Username = "testAthlete",
                Email = "testAthlete@email.com",
                Password = "testPassword"
            };

            var signUp2 = new SignUp
            {
                Id = 4,
                Username = "testAthlete2",
                Email = "testAthlete2@email.com",
                Password = "testPassword"
            };

            await client.PostAsJsonAsync($"/api/account/signup", signUp);
            await client.PostAsJsonAsync($"/api/account/signup", signUp2);

            await client.PostAsJsonAsync($"/api/athletes", new CreateAthlete
            {
                UserId = signUp.Id
            });
            await client.PostAsJsonAsync($"/api/athletes", new CreateAthlete
            {
                UserId = signUp2.Id
            });

            var response = await client.GetAsync($"/api/athletes");

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }


        [Fact]
        public async Task Post_ShouldReturnCreated_WhenDataIsValid()
        {
            var client = FreshClient();

            var signUp = new SignUp
            {
                Id = 3,
                Username = "testAthlete",
                Email = "testAthlete@email.com",
                Password = "testPassword"
            };

            await client.PostAsJsonAsync($"/api/account/signup", signUp);

            var response = await client.PostAsJsonAsync($"/api/athletes", new CreateAthlete
            {
                UserId = signUp.Id
            });

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
        }      

        [Fact]
        public async Task Post_ShouldReturnBadRequest_WhenAthleteDoesNotExist()
        {
            var client = FreshClient();
            var userId = 3;

            var response = await client.PostAsJsonAsync($"/api/athletes", new CreateAthlete
            {
                UserId = userId
            });

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WherUserIsAuthorizedAndUserIsAdmin()
        {
            var client = FreshClient();

            await client.AuthenticateAsync();


            var signUp = new SignUp
            {
                Id = 3,
                Username = "testAthlete",
                Email = "testAthlete@email.com",
                Password = "testPassword"
            };

            await client.PostAsJsonAsync($"/api/account/signup", signUp);

            await client.PostAsJsonAsync($"/api/athletes", new CreateAthlete
            {
                UserId = signUp.Id
            });

            var response = await client.DeleteAsJsonAsync($"/api/athletes", new DeleteAthlete
            {
                UserId = signUp.Id
            });

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_ShouldReturnUnauthorized_WhenUserIsNotAuthorized()
        {
            var client = FreshClient();
            var userId = 3;

            var response = await client.DeleteAsJsonAsync($"/api/athletes", new DeleteAthlete
            {
                UserId = userId
            });

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Delete_ShouldReturnForbidden_WhenUserIsAuthorizedButIsNotAdmin()
        {
            var client = FreshClient();
            var userId = 3;

            await client.AuthenticateUserAsync();

            var response = await client.DeleteAsJsonAsync($"/api/athletes", new DeleteAthlete
            {
                UserId = userId
            });

            response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_ShouldReturnBadRequest_WhenUserIsAuthorizedAndIsAdminButAthleteDoesNotExist()
        {
            var client = FreshClient();
            var userId = 3;

            await client.AuthenticateAsync();

            var response = await client.DeleteAsJsonAsync($"/api/athletes", new DeleteAthlete
            {
                UserId = userId
            });

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        private HttpClient FreshClient()
        {
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;

                        var db = scopedServices.GetRequiredService<FitwebContext>();
                        db.Athletes.RemoveRange(db.Athletes);
                        db.SaveChanges();
                    }
                });
            }).CreateClient();

            return client;
        }
    }
}
