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
    public class ExternalLoginControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private IConfiguration _configuration;

        public ExternalLoginControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var builder = new ConfigurationBuilder()
              .AddJsonFile("appsettings.Integration.json", optional: true, reloadOnChange: true);
            _configuration = builder.Build();
        }

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

        //    var jwt = await response.ReadAsString<JwtDto>();
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
