using Backend.Core.Entities;
using Backend.Infrastructure.EF.Configurations;
using Backend.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.EF
{
    public class FitwebContext : DbContext
    {
        private readonly SqlSettings _settings;

        public FitwebContext(DbContextOptions<FitwebContext> options, SqlSettings settings) : base(options)
        {
            _settings = settings;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_settings.InMemory)
            {
                optionsBuilder.UseInMemoryDatabase("InMemory");
                return;
            }

            optionsBuilder.UseSqlServer(_settings.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        }

        public DbSet<Athlete> Athletes { get; set; }

        public DbSet<Day> Days { get; set; }

        public DbSet<Exercise> Exercises { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<AthleteExercise> AthleteExercises { get; set; }

        public DbSet<AthleteProduct> AthleteProducts { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<CategoryOfProduct> CategoriesOfProduct { get; set; }

        public DbSet<PartOfBody> PartOfBodies { get; set; }
    }
}
