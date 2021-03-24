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
            builder.HasKey(ap => new { ap.AthleteId, ap.ProductId });

            builder.HasOne(ap => ap.Athlete)
                   .WithMany(a => a.AthleteProducts)
                   .HasForeignKey(ap => ap.AthleteId);

            builder.Ignore(ap => ap.Id);

            builder.Property(up => up.Weight).IsRequired().HasColumnType("float");
        }
    }
}
