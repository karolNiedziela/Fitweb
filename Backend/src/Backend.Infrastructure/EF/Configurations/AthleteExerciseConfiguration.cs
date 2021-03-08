using Backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.EF.Configurations
{
    public class AthleteExerciseConfiguration : BaseEntityConfiguration<AthleteExercise>
    {
        public override void Configure(EntityTypeBuilder<AthleteExercise> builder)
        {
            builder.HasKey(ae => new { ae.AthleteId, ae.ExerciseId });

            builder.HasOne(ae => ae.Athlete)
                   .WithMany(a => a.AthleteExercises)
                   .HasForeignKey(ae => ae.AthleteId);

            builder.HasOne(ae => ae.Exercise)
                   .WithMany(e => e.AthleteExercises)
                   .HasForeignKey(ae => ae.ExerciseId);

            builder.Ignore(ae => ae.Id);

            builder.Property(ue => ue.Weight).IsRequired().HasColumnType("float");
            builder.Property(ue => ue.NumberOfSets).IsRequired().HasColumnType("int");
            builder.Property(ue => ue.NumberOfReps).IsRequired().HasColumnType("int");
        }
    }
}
