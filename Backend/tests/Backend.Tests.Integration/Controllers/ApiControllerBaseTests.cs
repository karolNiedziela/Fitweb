using Backend.Api.Controllers;
using Backend.Core.Entities;
using Backend.Core.Repositories;
using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.EF;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Repositories;
using Backend.Infrastructure.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Integration.Controllers
{
    public class ApiControllerBaseTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        protected readonly HttpClient _client;

        protected ApiControllerBaseTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
                 {
                     builder.ConfigureServices(services =>
                     {
                         var descriptor = services
                             .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<FitwebContext>));
                         if (descriptor != null)
                         {
                             services.Remove(descriptor);
                         }

                         services.AddDbContext<FitwebContext>(options =>
                         {
                             options.UseInMemoryDatabase($"IntegrationTestDb");
                         });

                        services.Configure<IdentityOptions>(options =>
                        {
                            options.SignIn.RequireConfirmedEmail = false;
                        });
                     });
                 }).CreateClient(new WebApplicationFactoryClientOptions
                 {
                     AllowAutoRedirect = false
                 });
            
        }

        protected async Task AuthenticateAsync()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            var signUpResponse = await _client.PostAsJsonAsync("https://localhost:5001/api/account/signup", new SignUp
            {
                Username = "testAdmin",
                Email = "testAdminEmail@email.com",
                Password = "testAdminSecret",
                Role = RoleId.Admin.ToString()
            });

            var resultSingUpRespnse = await signUpResponse.Content.ReadAsStringAsync();

            var loginResponse = await _client.PostAsJsonAsync("https://localhost:5001/api/account/signin", new SignIn
            {
                Username = "testAdmin",
                Password = "testAdminSecret"
            });

            var resultLoginResponse = await loginResponse.Content.ReadAsStringAsync();

            var model = JsonConvert.DeserializeObject<JwtDto>(resultLoginResponse);

            return model.AccessToken;
        }
    }
}
