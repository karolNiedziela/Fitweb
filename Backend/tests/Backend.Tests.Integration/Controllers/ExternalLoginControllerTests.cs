using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.DTO;
using Backend.Tests.Integration.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Integration.Controllers
{
    public class ExternalLoginControllerTests : BaseIntegrationTest
    {
        private IConfiguration _configuration;

        //[Fact]
        //public async Task Post_ShouldReturnOk_WhenTokenIsValid()
        //{
        //    var testAccessToken = _configuration.GetSection("testAccessToken").Value;

        //    var facebookLogin = new FacebookLogin
        //    {
        //        AccessToken = testAccessToken
        //    };

        //    var response = await _client.PostAsJsonAsync("/api/facebook", facebookLogin);

        //    response.EnsureSuccessStatusCode();
        //    response.StatusCode.ShouldBe(HttpStatusCode.OK);

        //    var jwt = await response.Content.ReadAsAsync<JwtDto>();
        //    jwt.ShouldNotBeNull();
        //    jwt.Username.ShouldBe("fitweb_ogckbwt_tests@tfbnw.net");
        //}

        [Fact]
        public async Task Post_ShouldReturnBadRequest_WhenTokenIsInvalid()
        {
            var testAccessToken = "failToken";

            var facebookLogin = new FacebookLogin
            {
                AccessToken = testAccessToken
            };

            var response = await _client.PostAsJsonAsync("/api/facebook", facebookLogin);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
    }
}
