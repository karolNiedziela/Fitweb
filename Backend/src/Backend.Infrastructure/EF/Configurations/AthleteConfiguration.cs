using Backend.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.EF.Configurations
{
    public class AthleteConfiguration : BaseEntityConfiguration<Athlete>
    {
        public override void Configure(EntityTypeBuilder<Athlete> builder)
        {
            builder.HasOne(a => a.User);
        }
    }
}
