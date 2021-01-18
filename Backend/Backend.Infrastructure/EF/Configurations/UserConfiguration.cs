using Backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.EF.Configurations
{
    public class UserConfiguration : BaseEntityConfiguration<User>
    {       
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Username).IsRequired().HasColumnType("nvarchar(255)");
            builder.Property(u => u.Email).IsRequired().HasColumnType("nvarchar(255)");
            builder.Property(u => u.Password).IsRequired(false).HasColumnType("nvarchar(255)");
            builder.Property(u => u.IsExternalLoginProvider).IsRequired();
        }
    }
}
