using Backend.Core.Entities;
using Backend.Core.Enums;
using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.EF;
using Backend.Tests.Integration.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Integration.Controllers
{
    public class UsersControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public UsersControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Get_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // 1 is tesAdmin
            // 2 is testUser
            var client = FreshClient();
            var userId = 3;

            var response = await client.GetAsync($"/api/users/{userId}");
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_ShouldReturnUserDto_WhenUserExists()
        {
            var client = FreshClient();
            var user = await client.CreatePostAsync("https://localhost:5001/api/account/signup", new SignUp
            {
                Username = "test",
                Email = "test@email.com",
                Password = "testSecret",
                Role = RoleId.User.ToString()
            });

            var response = await client.GetAsync($"/api/users/{user.Id}");

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var dto = JsonConvert.DeserializeObject<UserDto>(await response.Content.ReadAsStringAsync());
            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(user.Id);
        }

        [Fact]
        public async Task GetAll_ShouldReturnIEnumerableUserDto()
        {
            var response = await _client.GetAsync("/api/users");

            response.EnsureSuccessStatusCode();

            var dto = await response.ReadAsString<IEnumerable<UserDto>>();
            dto.ShouldNotBeNull();
            dto.Count().ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenUserIsAuthorizedAndUserIsAdmin()
        {
            var client = FreshClient();

            var user = await client.CreatePostAsync("https://localhost:5001/api/account/signup", new SignUp
            {
                Username = "test",
                Email = "test@email.com",
                Password = "testSecret",
            });

            await client.AuthenticateAsync();

            var deleteUser = new DeleteUser
            {
                Id = user.Id
            };

            var response = await client.DeleteAsJsonAsync("/api/users", deleteUser);

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_ShouldReturnUnauthorized_WhenUserIsNotAuthorized()
        {
            var client = FreshClient();
            var deleteUser = new DeleteUser
            {
                Id = 1
            };

            var response = await client.DeleteAsJsonAsync("/api/users", deleteUser);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Delete_ShouldReturnBadRequest_WhenUserIsAdminButDataIsNotValid()
        {
            var client = FreshClient();

            await client.AuthenticateAsync();

            var response = await client.DeleteAsJsonAsync("/api/users", new DeleteUser { Id = 0 });

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_ShouldReturnForbidden_WhenUserIsAuthorizedButIsNotAdmin()
        {
            var client = FreshClient();

            await client.AuthenticateUserAsync();

            var response = await client.DeleteAsJsonAsync("/api/users", new DeleteUser { Id = 3 });

            response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
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
                        db.Users.RemoveRange(db.Users.Where(u => u.UserName != "testAdmin" && u.UserName != "testUser"));
                        db.SaveChanges();
                    }
                });
            }).CreateClient();

            return client;
        }

    }
}
