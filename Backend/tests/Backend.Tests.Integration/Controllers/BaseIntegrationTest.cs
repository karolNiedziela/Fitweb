using Backend.Core.Entities;
using Backend.Core.Enums;
using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Tests.Integration.Controllers
{
    public class BaseIntegrationTest : IDisposable
    {
        protected readonly HttpClient _client;
        private readonly IServiceProvider _serviceProvider;

        protected BaseIntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<FitwebContext>));

                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }
                        services.AddDbContext<FitwebContext>(options =>
                        {
                            options.UseInMemoryDatabase("IntegrationTestDb");
                        });

                        services.Configure<IdentityOptions>(options =>
                        {
                            options.SignIn.RequireConfirmedEmail = false;
                        });

                        var sp = services.BuildServiceProvider();

                        using (var scope = sp.CreateScope())
                        {
                            var scopedServices = scope.ServiceProvider;
                            var db = scopedServices.GetRequiredService<FitwebContext>();

                            var roleManager = scopedServices.GetRequiredService<RoleManager<Role>>();
                            var userManager = scopedServices.GetRequiredService<UserManager<User>>();
                            var roles = Enum.GetNames(typeof(RoleId)).ToList();

                            foreach (string role in roles)
                            {
                                if (!db.Roles.Any(r => r.Name == role))
                                {
                                    roleManager.CreateAsync(new Role(role));
                                }
                            }

                            if (!db.Users.Any(u => u.UserName == "testAdmin"))
                            {
                                var testAdmin = new User("testAdmin", "testAdminEmail@email.com");
                                userManager.CreateAsync(testAdmin, "testAdminSecret");
                                userManager.AddToRoleAsync(testAdmin, RoleId.Admin.ToString());
                            }

                            if (!db.Users.Any(u => u.UserName == "testUser"))
                            {
                                var testUser = new User("testUser", "testUserEmail@email.com");
                                userManager.CreateAsync(testUser, "testUserSecret");
                                userManager.AddToRoleAsync(testUser, RoleId.User.ToString());
                            }
                            if (!db.CategoriesOfProduct.Any())
                            {
                                db.CategoriesOfProduct.Add(new CategoryOfProduct
                                {
                                    Name = CategoryOfProductId.Meat
                                });

                                db.SaveChanges();
                            }

                            if (!db.PartOfBodies.Any())
                            {
                                db.PartOfBodies.Add(new PartOfBody
                                {
                                    Name = PartOfBodyId.Chest
                                });

                                db.SaveChanges();
                            }

                            db.Database.EnsureCreated();
                        }

                    });
                });

            _serviceProvider = appFactory.Services;
            _client = appFactory.CreateClient();
        }

        protected async Task AuthenticateAdminAsync()
        {
            var loginResponse = await _client.PostAsJsonAsync("https://localhost:5001/api/account/signin", new SignIn
            {
                Username = "testAdmin",
                Password = "testAdminSecret"
            });

            await SetTokenAsync(loginResponse);
        }

        protected async Task AuthenticateUserAsync()
        {

            var loginResponse = await _client.PostAsJsonAsync("https://localhost:5001/api/account/signin", new SignIn
            {
                Username = "testUser",
                Password = "testUserSecret"
            });

            await SetTokenAsync(loginResponse);
        }

        private async Task SetTokenAsync(HttpResponseMessage message)
        {
            var model = await message.Content.ReadAsAsync<JwtDto>();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.AccessToken);
        }

        public void Dispose()
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<FitwebContext>();
            context.Database.EnsureDeleted();
        }

    }
}
