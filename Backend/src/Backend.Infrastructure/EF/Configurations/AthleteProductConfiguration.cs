using Backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.EF.Configurations
{
    public class AthleteProductConfiguration : BaseEntityConfiguration<AthleteProduct>
    {
        public override void Configure(EntityTypeBuilder<AthleteProduct> builder)
        {
            builder.HasOne(ap => ap.Athlete)
                   .WithMany(a => a.AthleteProducts)
                   .HasForeignKey(ap => ap.AthleteId);

            builder.Property(up => up.Weight).IsRequired().HasColumnType("float");

            builder.HasIndex(ap => new { ap.AthleteId, ap.ProductId })
                   .IsUnique(false);
        }
    }
}
