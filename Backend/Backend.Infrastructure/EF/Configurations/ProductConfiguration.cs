using Backend.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.EF.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasColumnType("nvarchar(200)");
            builder.Property(x => x.Calories).IsRequired().HasColumnType("float");
            builder.Property(x => x.Proteins).IsRequired().HasColumnType("float");
            builder.Property(x => x.Carbohydrates).IsRequired().HasColumnType("float");
            builder.Property(x => x.Fats).IsRequired().HasColumnType("float");

            builder.HasData(
                new Product { Id = 1, Name = "Banana", Calories = 95.0, Proteins = 1.0, Carbohydrates = 23.5, Fats = 0.3 },
                new Product { Id = 2, Name = "Peach", Calories = 46.0, Proteins = 1.0, Carbohydrates = 11.9, Fats = 0.2 },
                new Product { Id = 3, Name = "Sugar", Calories = 405.0, Proteins = 0.0, Carbohydrates = 99.8, Fats = 0.0 },
                new Product { Id = 4, Name = "Brown sugar", Calories = 380.0, Proteins = 0.1, Carbohydrates = 98.0, Fats = 0.0 },
                new Product { Id = 5, Name = "Lemon", Calories = 36.0, Proteins = 0.8, Carbohydrates = 9.5, Fats = 0.3 },
                new Product { Id = 6, Name = "Tiger shrimp", Calories = 92.0, Proteins = 22.0, Carbohydrates = 1.0, Fats = 0.0 },
                new Product { Id = 7, Name = "Smoked salmon", Calories = 162.0, Proteins = 21.5, Carbohydrates = 0.0, Fats = 8.4 },
                new Product { Id = 8, Name = "Raw salmon", Calories = 201.0, Proteins = 19.9, Carbohydrates = 0.0, Fats = 13.6 },
                new Product { Id = 9, Name = "Turkey tenderloin", Calories = 105.0, Proteins = 15.8, Carbohydrates = 0.1, Fats = 4.6 },
                new Product { Id = 10, Name = "Chicken breast sirloin", Calories = 93.0, Proteins = 20.4, Carbohydrates = 0.0, Fats = 1.2 },
                new Product { Id = 11, Name = "Tomato", Calories = 15.0, Proteins = 0.9, Carbohydrates = 3.6, Fats = 0.2 },
                new Product { Id = 12, Name = "Mushrooms", Calories = 17.0, Proteins = 2.7, Carbohydrates = 2.6, Fats = 0.4 },
                new Product { Id = 13, Name = "Rice", Calories = 344.0, Proteins = 6.7, Carbohydrates = 78.9, Fats = 0.7 },
                new Product { Id = 14, Name = "Salami", Calories = 540.0, Proteins = 21.9, Carbohydrates = 0.9, Fats = 50.6 },
                new Product { Id = 15, Name = "Lettuce", Calories = 14.0, Proteins = 1.4, Carbohydrates = 2.9, Fats = 0.2 },
                new Product { Id = 16, Name = "Roasted pork loin", Calories = 291.0, Proteins = 30.4, Carbohydrates = 0.7, Fats = 18.7 },
                new Product { Id = 17, Name = "Cheddar cheese", Calories = 391.0, Proteins = 27.1, Carbohydrates = 0.1, Fats = 31.7 }
            );
        }
    }
}
