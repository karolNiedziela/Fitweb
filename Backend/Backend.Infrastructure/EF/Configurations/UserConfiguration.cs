using Backend.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.EF.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {       
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Username).IsRequired().HasColumnType("nvarchar(200)");
            builder.Property(x => x.Email).IsRequired().HasColumnType("nvarchar(200)");
            builder.Property(x => x.Password).IsRequired().HasColumnType("nvarchar(200)");
            builder.Property(x => x.Salt).IsRequired().HasColumnType("nvarchar(200)");
            builder.Property(x => x.CreatedAt).IsRequired().HasColumnType("date");
            builder.Property(x => x.UpdatedAt).IsRequired().HasColumnType("date");
        }
    }
}
