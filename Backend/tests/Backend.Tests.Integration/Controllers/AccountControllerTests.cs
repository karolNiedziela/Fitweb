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
    public class AccountControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public AccountControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Get_ShouldReturnOk_WhenUserIsAuthorized()
        {
            var client = FreshClient();
            await client.AuthenticateUserAsync();

            var response = await client.GetAsync("/api/account/me");

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var user = await response.ReadAsString<UserDto>();
            user.Email.ShouldBe("testUserEmail@email.com");
            user.UserName.ShouldBe("testUser");
            user.Role.ShouldBe(RoleId.User.ToString());
        }

        [Fact]
        public async Task Get_ShouldReturnUnauthorized_WhenUserIsNotAuthorized()
        {
            var client = FreshClient();

            var response = await client.GetAsync("/api/account/me");

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task SigIn_ShouldReturnOk_WhenSignInDataIsValid()
        {
            var client = FreshClient();

            var user = new SignUp
            {
                Username = "UserTest",
                Email = "user@email.com",
                Password = "Secret1="
            };

            await client.PostAsJsonAsync("/api/account/signup", user);

            var response = await client.PostAsJsonAsync("/api/account/signin", new SignIn
            {
                Username = user.Username,
                Password = user.Password
            });

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var jwt = await response.ReadAsString<JwtDto>();

            jwt.Username.ShouldBe(user.Username);
            jwt.Role.ShouldBe(RoleId.User.ToString());     
        }

        [Fact]
        public async Task SignIn_ShouldReturnBadRequest_WhenSignInDataIsInvalid()
        {
            var client = FreshClient();
            var signIn = new SignIn
            {
                Username = "UserTest",
                Password = "Secret1="
            };

            var response = await client.PostAsJsonAsync("api/account/signin", signIn);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task SignUp_ShouldReturnCreate_WhenSignUpDataIsValid()
        {
            var client = FreshClient();
            var signUp = new SignUp
            {
                Username = "UserTest",
                Email = "user@email.com",
                Password = "Secret1="
            };

            var response = await client.PostAsJsonAsync("api/account/signup", signUp);

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var result = await response.ReadAsString<SignUp>();
        }

        [Fact]
        public async Task SignUp_ShouldReturnBadRequest_WhenSignUpDataIsInvalid()
        {
            var client = FreshClient();
            var signUp = new SignUp
            {
                Username = "use", // too short username
                Email = "useremail", // invalid email format
                Password = "123" // too short
            };

            var response = await client.PostAsJsonAsync("api/account/signup", signUp);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ChangePassword_ShouldReturnNoContent_WhenUserIsAuthorized()
        {
            var client = FreshClient();

            var signUp = new SignUp
            {
                Username = "changeUser",
                Email = "change@email.com",
                Password = "change123"
            };

            await client.PostAsJsonAsync("api/account/signup", signUp);

            var signIn = new SignIn
            {
                Username = "changeUser",
                Password = "change123"
            };

            var signInResponse = await client.PostAsJsonAsync("/api/account/signin", signIn);

            var signInResult = await signInResponse.Content.ReadAsStringAsync();

            var jwt = JsonConvert.DeserializeObject<JwtDto>(signInResult);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.AccessToken);

            var changePassword = new ChangePassword
            {
                OldPassword = "change123",
                NewPassword = "newChange"
            };

            var response = await client.PatchAsJsonAsync("api/account/changepassword", changePassword);

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task ChangePassword_ShouldReturnUnauthorized_WhenUserIsNotAuthorized()
        {
            var client = FreshClient();
            var changePassword = new ChangePassword
            {
                OldPassword = "Secret1",
                NewPassword = "Secret2"
            };

            var response = await client.PatchAsJsonAsync("api/account/changepassword", changePassword);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ChangePassword_ShouldReturnBadRequest_WhenPasswordInInvalid()
        {
            var client = FreshClient();

            await client.AuthenticateUserAsync();

            var changePassword = new ChangePassword
            {
                OldPassword = "Secret1",
                NewPassword = "Secret2"
            };

            var response = await client.PatchAsJsonAsync("api/account/changepassword", changePassword);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ForgotPassword_ShouldReturnNoContent_WhenEmailIsValid()
        {
            var client = FreshClient();

            var forgotPassword = new ForgotPassword
            {
                Email = "testUserEmail@email.com"
            };

            var response = await client.PostAsJsonAsync("api/account/forgotpassword", forgotPassword);

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task ForgotPassword_ShouldReturnBadRequest_WhenEmailIsNotValid()
        {
            var client = FreshClient();

            var forgotPassword = new ForgotPassword
            {
                Email = "nonExisting"
            };

            var response = await client.PostAsJsonAsync("api/account/forgotpassword", forgotPassword);

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
                        db.Users.RemoveRange(db.Users.Where(u => u.UserName != "testAdmin" && u.UserName != "testUser"));
                        db.SaveChanges();
                    }
                });
            }).CreateClient();

            return client;
        }

    }
}
