using Backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend.Infrastructure.EF.Configurations
{
    public class CategoryOfProductConfiguration : IEntityTypeConfiguration<CategoryOfProduct>
    {
        public void Configure(EntityTypeBuilder<CategoryOfProduct> builder)
        {
            builder.HasKey(cop => cop.Id);

            builder.Property(cop => cop.Name)
                   .HasConversion(
                    cop => cop.ToString(),
                    cop => (CategoryOfProductId)Enum.Parse(typeof(CategoryOfProductId), cop)
                    );

            builder.HasData(
                Enum.GetValues(typeof(CategoryOfProductId))
                .Cast<CategoryOfProductId>()
                .Select(cop => new CategoryOfProduct
                {
                    Id = (int)cop,
                    Name = cop
                })
           );

        }
    }
}
