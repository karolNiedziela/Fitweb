using Backend.Core.Entities;
using Backend.Infrastructure.EF;
using Backend.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitweb.Tests.Unit
{
    public class FitwebSeedDataFixture : IDisposable
    {
        public FitwebContext FitwebContext { get; private set; }

        public FitwebSeedDataFixture()
        {
            var options = new DbContextOptionsBuilder<FitwebContext>().Options;
            var sqlSettings = new SqlSettings();
            sqlSettings.InMemory = true;

            FitwebContext = new FitwebContext(options, sqlSettings);

            FitwebContext.Users.Add(new User
            {
                Id = 1,
                Username = "user1",
                Email = "user1@email.com",
                Password = "secret",
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                IsExternalLoginProvider = false
            });

            FitwebContext.Users.Add(new User
            {
                Id = 2,
                Username = "user2",
                Email = "user2@email.com",
                Password = "secret",
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                IsExternalLoginProvider = false
            });

            FitwebContext.Products.Add(new Product
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
            });

            FitwebContext.Products.Add(new Product
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

            FitwebContext.CategoriesOfProduct.Add(new CategoryOfProduct
            {
                Id = 1,
                Name = CategoryOfProductId.Meat
            });

            FitwebContext.Exercises.Add(new Exercise
            {
                Id = 1,
                Name = "exercise1",
                PartOfBodyId = 1,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            });
            FitwebContext.Exercises.Add(new Exercise
            {
                Id = 2,
                Name = "exercise2",
                PartOfBodyId = 1,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            });

            FitwebContext.PartOfBodies.Add(new PartOfBody
            {
                Id = 1,
                Name = PartOfBodyId.Chest
            });


            FitwebContext.SaveChanges();
        }

        public void Dispose()
        {
            FitwebContext.Database.EnsureDeleted();
            FitwebContext.Dispose();
        }
    }
}
