using Backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
            builder.HasKey(r => r.Id);

            builder.HasMany(u => u.UserRoles)
               .WithOne(ur => ur.Role)
               .HasForeignKey(ur => ur.RoleId)
               .IsRequired();

            //builder.Property(r => r.Name)
            //       .HasConversion(new EnumToStringConverter<RoleId>());
            ///*r => r.ToString(),
            //r => (RoleId)Enum.Parse(typeof(RoleId), r));*/

            builder.HasData(
                Enum.GetValues(typeof(RoleId))
                    .Cast<RoleId>()
                    .Select(r => new Role()
                    {
                        Id = (int)r,
                        Name = r.ToString(),
                        NormalizedName = r.ToString().ToUpper()
                    }));
                        
        }
    }
}
