using Backend.Core.Entities;
using Backend.Infrastructure.EF.Configurations;
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
    public class FitwebContext : IdentityDbContext<User, Role, int>
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
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles", "dbo");
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
    }
}
