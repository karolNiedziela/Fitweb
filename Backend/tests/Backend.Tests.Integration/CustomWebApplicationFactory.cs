using Backend.Core.Entities;
using Backend.Core.Enums;
using Backend.Infrastructure.EF;
using Backend.Infrastructure.Services;
using Backend.Tests.Integration.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Tests.Integration
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<FitwebContext>));

                services.Remove(descriptor);

                services.AddDbContext<FitwebContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryTestDb");
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
        }
    }
}
