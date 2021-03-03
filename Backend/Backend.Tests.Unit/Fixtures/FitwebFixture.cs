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
            var sqlSettings = new SqlSettings
            {
                InMemory = true
            };

            FitwebContext = new FitwebContext(options, sqlSettings);

            FitwebContext.Users.AddRange(new User
            {
                Id = 1,
                UserName = "user1",
                Email = "user1@email.com",
                PasswordHash = "secret",
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                IsExternalLoginProvider = false
            },
            new User
            {
                Id = 2,
                UserName = "user2",
                Email = "user2@email.com",
                PasswordHash = "secret",
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                IsExternalLoginProvider = false
            },
            new User
            {
                Id = 3,
                UserName = "user3",
                Email = "user3@email.com",
                PasswordHash = "secret",
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                IsExternalLoginProvider = false
            }
            );

            FitwebContext.Roles.Add(new Role
            {
                Name = RoleId.User
            });


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
            },
            new Product
            {
                Id = 3,
                Name = "product3",
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

            FitwebContext.Athletes.AddRange(new Athlete
            {
                Id = 1,
                UserId = 1
            },
            new Athlete
            {
                Id = 2,
                UserId = 2
            });

            FitwebContext.AthleteProducts.AddRange(
                new AthleteProduct
                {
                   AthleteId = 1,
                   ProductId = 1,
                   Weight = 100
                },
                new AthleteProduct 
                {
                    AthleteId = 2,
                    ProductId = 2,
                    Weight = 200
                },
                new AthleteProduct
                {
                    AthleteId = 1,
                    ProductId = 2,
                    Weight = 300
                }
             );

            FitwebContext.Days.AddRange(           
                new Day
                {
                    Id = 1,
                    Name = DayId.Monday
                },
                new Day
                {
                    Id = 2,
                    Name = DayId.Tuesday
                },
                new Day
                {
                    Id = 3,
                    Name = DayId.Wednesday
                },
                new Day
                {
                    Id = 4,
                    Name = DayId.Thursday
                },
                new Day
                {
                    Id = 5,
                    Name = DayId.Friday
                },
                new Day
                {
                    Id = 6,
                    Name = DayId.Saturday
                },
                new Day
                {
                    Id = 7,
                    Name = DayId.Saturday
                }
            );

            FitwebContext.AthleteExercises.AddRange(
                new AthleteExercise
                {
                    Id = 1,
                    AthleteId = 1,
                    ExerciseId = 1,
                    Weight = 30,
                    NumberOfSets = 3,
                    NumberOfReps = 10,
                    DayId = 1
                },
                new AthleteExercise
                {
                    Id = 2,
                    AthleteId = 1,
                    ExerciseId = 2,
                    Weight = 150,
                    NumberOfSets = 5,
                    NumberOfReps = 5,
                    DayId = 5
                },
                new AthleteExercise
                {
                    Id = 3,
                    AthleteId = 2,
                    ExerciseId = 1,
                    Weight = 100,                   
                    NumberOfSets = 4,
                    NumberOfReps = 12,
                    DayId = 6
                }
             );

            FitwebContext.RefreshTokens.AddRange(
                new RefreshToken(3, "randomTestToken", DateTime.UtcNow),
                new RefreshToken(1, "randomTestToken2", DateTime.UtcNow)
            );


            FitwebContext.SaveChanges();

        }

        public void Dispose()
        {
            FitwebContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
