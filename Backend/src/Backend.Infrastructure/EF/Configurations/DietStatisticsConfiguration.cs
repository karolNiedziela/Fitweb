using Backend.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.EF.Configurations
{
    public class DietStatisticsConfiguration : BaseEntityConfiguration<DietStat>
    {
        public override void Configure(EntityTypeBuilder<DietStat> builder)
        {
            builder.Property(cd => cd.TotalCalories).IsRequired();
            builder.Property(cd => cd.TotalProteins).IsRequired();
            builder.Property(cd => cd.TotalCarbohydrates).IsRequired();
            builder.Property(cd => cd.TotalFats).IsRequired();
        }
    }
}
