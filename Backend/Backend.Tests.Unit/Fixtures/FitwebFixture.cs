using Backend.Core.Entities;
using Backend.Infrastructure.EF;
using Backend.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Tests.Unit.Fixtures
{
    public class FitwebFixture : IDisposable
    {
        public FitwebContext FitwebContext { get; private set; }

        public FitwebFixture()
        {
            var options = new DbContextOptionsBuilder<FitwebContext>().Options;
            var sqlSettings = new SqlSettings();
            sqlSettings.InMemory = true;

            FitwebContext = new FitwebContext(options, sqlSettings);



            FitwebContext.Users.AddRange(new User
            {
                Id = 1,
                Username = "user1",
                Email = "user1@email.com",
                Password = "secret",
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                IsExternalLoginProvider = false
            },
            new User
            {
                Id = 2,
                Username = "user2",
                Email = "user2@email.com",
                Password = "secret",
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                IsExternalLoginProvider = false
            },
            new User
            {
                Id = 3,
                Username = "user3",
                Email = "user3@email.com",
                Password = "secret",
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                IsExternalLoginProvider = false
            }
            );

            FitwebContext.Roles.Add(new Role
            {
                Name = RoleId.User
            });

            var users = FitwebContext.Users.ToList();
            var roles = FitwebContext.Roles.ToList();

             FitwebContext.UserRoles.AddRange(new UserRole
             {
                 UserId = 1,
                 RoleId = 1,
             },
             new UserRole
             {
                 UserId = 2,
                 RoleId = 1,
             },
             new UserRole
             {
                 UserId = 3,
                 RoleId = 1,
             });

            FitwebContext.CategoriesOfProduct.Add(new CategoryOfProduct
            {
                Name = CategoryOfProductId.Meat
            });

            FitwebContext.Products.AddRange(new Product
            {
                Id = 1,
                Name = "product1",
                Calories = 100,
                Proteins = 10,
                Carbohydrates = 10,
                Fats = 10,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                CategoryOfProductId = 1
            },
            new Product
            {
                Id = 2,
                Name = "product2",
                Calories = 500,
                Proteins = 25,
                Carbohydrates = 70,
                Fats = 5,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                CategoryOfProductId = 1
            });

            FitwebContext.PartOfBodies.Add(new PartOfBody
            {
                Id = 1,
                Name = PartOfBodyId.Chest
            });

            FitwebContext.Exercises.AddRange(new Exercise
            {
                Id = 1,
                Name = "exercise1",
                PartOfBodyId = 1,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            },
            new Exercise
            {
                Id = 2,
                Name = "exercise2",
                PartOfBodyId = 1,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            });


            FitwebContext.RefreshTokens.AddRange(
                new RefreshToken(3, "randomTestToken", DateTime.UtcNow),
                new RefreshToken(1, "randomTestToken2", DateTime.UtcNow)
            );


            FitwebContext.SaveChanges();

        }

        public void Dispose()
        {
            FitwebContext.Dispose();
        }
    }
}
