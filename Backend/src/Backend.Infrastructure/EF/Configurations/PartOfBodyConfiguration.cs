using Backend.Core.Entities;
using Backend.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend.Infrastructure.EF.Configurations
{
    public class PartOfBodyConfiguration : IEntityTypeConfiguration<PartOfBody>
    {
        public void Configure(EntityTypeBuilder<PartOfBody> builder)
        {
            builder.HasKey(pob => pob.Id);

            builder.Property(pob => pob.Name)
                   .HasConversion(
                    pob => pob.ToString(),
                    pob => (PartOfBodyId)Enum.Parse(typeof(PartOfBodyId), pob));

            builder.HasData(
                Enum.GetValues(typeof(PartOfBodyId))
                .Cast<PartOfBodyId>()
                .Select(pob => new PartOfBody
                {
                    Id = (int)pob,
                    Name = pob
                }));
        }
    }
}
