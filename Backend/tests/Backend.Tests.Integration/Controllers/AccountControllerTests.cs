﻿using Backend.Core.Enums;
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
    public class AccountControllerTests : BaseIntegrationTest
    {
        [Fact]
        public async Task Get_ShouldReturnOk_WhenUserIsAuthorized()
        {
            await AuthenticateUserAsync();

            var response = await _client.GetAsync("/api/account/me");

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var user = await response.Content.ReadAsAsync<UserDto>();
            user.Email.ShouldBe("testUserEmail@email.com");
            user.UserName.ShouldBe("testUser");
            user.Role.ShouldBe(RoleId.User.ToString());
        }

        [Fact]
        public async Task Get_ShouldReturnUnauthorized_WhenUserIsNotAuthorized()
        {
            var response = await _client.GetAsync("/api/account/me");

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task SigIn_ShouldReturnOk_WhenSignInDataIsValid()
        {
            var user = new SignUp
            {
                Username = "UserTest",
                Email = "user@email.com",
                Password = "Secret1="
            };

            await _client.PostAsJsonAsync("/api/account/signup", user);

            var response = await _client.PostAsJsonAsync("/api/account/signin", new SignIn
            {
                Username = user.Username,
                Password = user.Password
            });

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var jwt = await response.Content.ReadAsAsync<JwtDto>();

            jwt.Username.ShouldBe(user.Username);
            jwt.Role.ShouldBe(RoleId.User.ToString());     
        }

        [Fact]
        public async Task SignIn_ShouldReturnBadRequest_WhenSignInDataIsInvalid()
        {
            var signIn = new SignIn
            {
                Username = "UserTest",
                Password = "Secret1="
            };

            var response = await _client.PostAsJsonAsync("api/account/signin", signIn);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task SignUp_ShouldReturnCreate_WhenSignUpDataIsValid()
        {
            var signUp = new SignUp
            {
                Username = "UserTest",
                Email = "user@email.com",
                Password = "Secret1="
            };

            var response = await _client.PostAsJsonAsync("api/account/signup", signUp);

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var result = await response.Content.ReadAsAsync<SignUp>();
        }

        [Fact]
        public async Task SignUp_ShouldReturnBadRequest_WhenSignUpDataIsInvalid()
        {
            var signUp = new SignUp
            {
                Username = "use", // too short username
                Email = "useremail", // invalid email format
                Password = "123" // too short
            };

            var response = await _client.PostAsJsonAsync("api/account/signup", signUp);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ChangePassword_ShouldReturnNoContent_WhenUserIsAuthorized()
        {
            var signUp = new SignUp
            {
                Username = "changeUser",
                Email = "change@email.com",
                Password = "change123"
            };

            await _client.PostAsJsonAsync("api/account/signup", signUp);

            var signIn = new SignIn
            {
                Username = "changeUser",
                Password = "change123"
            };

            var signInResponse = await _client.PostAsJsonAsync("/api/account/signin", signIn);

            var signInResult = await signInResponse.Content.ReadAsStringAsync();

            var jwt = JsonConvert.DeserializeObject<JwtDto>(signInResult);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.AccessToken);

            var changePassword = new ChangePassword
            {
                OldPassword = "change123",
                NewPassword = "newChange"
            };

            var response = await _client.PatchAsJsonAsync("api/account/changepassword", changePassword);

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task ChangePassword_ShouldReturnUnauthorized_WhenUserIsNotAuthorized()
        {
            var changePassword = new ChangePassword
            {
                OldPassword = "Secret1",
                NewPassword = "Secret2"
            };

            var response = await _client.PatchAsJsonAsync("api/account/changepassword", changePassword);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ChangePassword_ShouldReturnBadRequest_WhenPasswordInInvalid()
        {
            await AuthenticateUserAsync();

            var changePassword = new ChangePassword
            {
                OldPassword = "Secret1",
                NewPassword = "Secret2"
            };

            var response = await _client.PatchAsJsonAsync("api/account/changepassword", changePassword);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ForgotPassword_ShouldReturnNoContent_WhenEmailIsValid()
        {
            var forgotPassword = new ForgotPassword
            {
                Email = "testUserEmail@email.com"
            };

            var response = await _client.PostAsJsonAsync("api/account/forgotpassword", forgotPassword);

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task ForgotPassword_ShouldReturnBadRequest_WhenEmailIsNotValid()
        {
            var forgotPassword = new ForgotPassword
            {
                Email = "nonExisting"
            };

            var response = await _client.PostAsJsonAsync("api/account/forgotpassword", forgotPassword);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
    }
}
