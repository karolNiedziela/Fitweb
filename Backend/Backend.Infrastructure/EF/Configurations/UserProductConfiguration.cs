using Backend.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.EF.Configurations
{
    public class UserProductConfiguration : IEntityTypeConfiguration<UserProduct>
    {
        public void Configure(EntityTypeBuilder<UserProduct> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.Products)
                   .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Product)
                   .WithMany(x => x.Users)
                   .HasForeignKey(x => x.ProductId);

            builder.Property(x => x.Weight).IsRequired().HasColumnType("float");
            builder.Property(x => x.Calories).IsRequired().HasColumnType("float");
            builder.Property(x => x.Proteins).IsRequired().HasColumnType("float");
            builder.Property(x => x.Carbohydrates).IsRequired().HasColumnType("float");
            builder.Property(x => x.Fats).IsRequired().HasColumnType("float");
            builder.Property(x => x.AddedAt).IsRequired().HasColumnType("date");
        }
    }
}
