using Backend.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend.Infrastructure.EF.Configurations
{
    public class DayConfiguration : IEntityTypeConfiguration<Day>
    {
        public void Configure(EntityTypeBuilder<Day> builder)
        {
            builder.Property(x => x.Id).HasConversion<int>();
            builder.HasData(
                Enum.GetValues(typeof(Days))
                .Cast<Days>()
                .Select(x => new Day()
                {
                    Id = x,
                    Name = x.ToString()
                })
            );
        }
    }
}
