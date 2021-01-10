using Backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.EF.Configurations
{
    public class ProductConfiguration : BaseEntityConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(p => p.CategoryOfProduct);

            builder.Property(p => p.Name).IsRequired().HasColumnType("nvarchar(200)");
            builder.Property(p => p.Calories).IsRequired().HasColumnType("float");
            builder.Property(p => p.Proteins).IsRequired().HasColumnType("float");
            builder.Property(p => p.Carbohydrates).IsRequired().HasColumnType("float");
            builder.Property(p => p.Fats).IsRequired().HasColumnType("float");     
        }
    }
}
