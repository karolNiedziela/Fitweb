using Backend.Core.Entities;
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
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                   .HasConversion(
                    d => d.ToString(),
                    d => (DayId)Enum.Parse(typeof(DayId), d));

            builder.HasData(
                Enum.GetValues(typeof(DayId))
                .Cast<DayId>()
                .Select(d => new Day()
                {
                    Id = (int)d,
                    Name = d
                })
            );
        }
    }
}
