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
    public class UsersControllerTests : BaseIntegrationTest
    {
        [Fact]
        public async Task Get_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // 1 is testAdmin
            // 2 is testUser
            await AuthenticateAdminAsync();
            var userId = 3;

            var response = await _client.GetAsync($"/api/users/{userId}");
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_ShouldReturnUserDto_WhenUserExists()
        {
            await AuthenticateAdminAsync();
            var user = await _client.CreatePostAsync("https://localhost:5001/api/account/signup", new SignUp
            {
                Username = "test",
                Email = "test@email.com",
                Password = "testSecret",
                Role = RoleId.User.ToString()
            });

            var response = await _client.GetAsync($"/api/users/{user.Id}");

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var dto = JsonConvert.DeserializeObject<UserDto>(await response.Content.ReadAsStringAsync());
            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(user.Id);
        }

        [Fact]
        public async Task GetAll_ShouldReturnIEnumerableUserDto()
        {
            await AuthenticateAdminAsync();
            var response = await _client.GetAsync("/api/users");

            response.EnsureSuccessStatusCode();

            var dto = await response.Content.ReadAsAsync<IEnumerable<UserDto>>();
            dto.ShouldNotBeNull();
            dto.Count().ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenUserIsAuthorizedAndUserIsAdmin()
        {
            var user = await _client.CreatePostAsync("https://localhost:5001/api/account/signup", new SignUp
            {
                Username = "test",
                Email = "test@email.com",
                Password = "testSecret",
            });

            await AuthenticateAdminAsync();

            var deleteUser = new DeleteUser
            {
                Id = user.Id
            };

            var response = await _client.DeleteAsJsonAsync("/api/users", deleteUser);

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_ShouldReturnUnauthorized_WhenUserIsNotAuthorized()
        {
            var deleteUser = new DeleteUser
            {
                Id = 1
            };

            var response = await _client.DeleteAsJsonAsync("/api/users", deleteUser);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Delete_ShouldReturnBadRequest_WhenUserIsAdminButDataIsNotValid()
        {
            await AuthenticateAdminAsync();

            var response = await _client.DeleteAsJsonAsync("/api/users", new DeleteUser { Id = 0 });

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_ShouldReturnForbidden_WhenUserIsAuthorizedButIsNotAdmin()
        {
            await AuthenticateUserAsync();

            var response = await _client.DeleteAsJsonAsync("/api/users", new DeleteUser { Id = 3 });

            response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

    }
}
