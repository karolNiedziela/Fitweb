using Backend.Core.Entities;
using Backend.Infrastructure.EF.Configurations;
using Backend.Infrastructure.EF.SeedData;
using Backend.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.EF
{
    public class FitwebContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>,
        UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {

        public FitwebContext(DbContextOptions<FitwebContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
            builder.Entity<User>().ToTable("Users", "dbo");
            builder.Entity<Role>().ToTable("Roles", "dbo");
            builder.Entity<UserRole>().ToTable("UserRoles", "dbo");

            foreach (var item in SeedExercises.Get())
            {
                builder.Entity<Exercise>().HasData(item);
            }

            foreach (var item in SeedProducts.Get())
            {
                builder.Entity<Product>().HasData(item);
            }
        }

        public DbSet<Athlete> Athletes { get; set; }

        public DbSet<Day> Days { get; set; }

        public DbSet<Exercise> Exercises { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<AthleteExercise> AthleteExercises { get; set; }

        public DbSet<AthleteProduct> AthleteProducts { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<CategoryOfProduct> CategoriesOfProduct { get; set; }

        public DbSet<PartOfBody> PartOfBodies { get; set; }

        public DbSet<CaloricDemand> CaloricDemands { get; set; }

        public DbSet<AthleteDietStats> AthleteDietStats { get; set; }

        public DbSet<DietStat> DietStats { get; set; }
    }
}
