using Backend.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.EF.Configurations
{
    public class AthleteDietStatisticsConfiguration : BaseEntityConfiguration<AthleteDietStats>
    {
        public override void Configure(EntityTypeBuilder<AthleteDietStats> builder)
        {
            builder.HasIndex(ads => new {ads.UserId , ads.DietStatId })
                .IsUnique(false);

            builder.HasOne(ads => ads.Athlete)
                   .WithMany(a => a.AthleteDietStats)
                   .HasForeignKey(ads => ads.UserId);
        }
    }
}
