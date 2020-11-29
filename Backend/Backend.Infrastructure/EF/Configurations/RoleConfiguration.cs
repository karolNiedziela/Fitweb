using Backend.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend.Infrastructure.EF.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(x => x.Id).HasConversion<int>();
            builder.HasData(
                Enum.GetValues(typeof(Roles))
                .Cast<Roles>()
                .Select(x => new Role()
                {
                    Id = x,
                    Name = x.ToString()
                })
            );
        }
    }
}
