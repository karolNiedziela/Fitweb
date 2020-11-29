using Backend.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.EF.Configurations
{
    public class DietGoalsConfiguration : IEntityTypeConfiguration<DietGoals>
    {
        public void Configure(EntityTypeBuilder<DietGoals> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TotalCalories).IsRequired().HasColumnType("float");
            builder.Property(x => x.Proteins).IsRequired().HasColumnType("float");
            builder.Property(x => x.Carbohydrates).IsRequired().HasColumnType("float");
            builder.Property(x => x.Fats).IsRequired().HasColumnType("float");

        }
    }
}
