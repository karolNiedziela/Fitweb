using Backend.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.EF.Configurations
{
    public class CaloricDemandConfiguration : BaseEntityConfiguration<CaloricDemand>
    {
        public override void Configure(EntityTypeBuilder<CaloricDemand> builder)
        {
            builder.Property(cd => cd.TotalCalories).IsRequired();
            builder.Property(cd => cd.Proteins).IsRequired();
            builder.Property(cd => cd.Carbohydrates).IsRequired();
            builder.Property(cd => cd.Fats).IsRequired();
        }
    }
}
