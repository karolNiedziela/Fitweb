using Backend.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.EF.Configurations
{
    public class UserExerciseConfiguration : IEntityTypeConfiguration<UserExercise>
    {
        public void Configure(EntityTypeBuilder<UserExercise> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.Exercises)
                   .HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.Exercise)
                   .WithMany(x => x.UserExercises)
                   .HasForeignKey(x => x.ExerciseId);

            builder.Property(x => x.Weight).IsRequired().HasColumnType("float");
            builder.Property(x => x.NumberOfSets).IsRequired().HasColumnType("int");
            builder.Property(x => x.NumberOfReps).IsRequired().HasColumnType("int");
            builder.Property(x => x.AddedAt).IsRequired().HasColumnType("date");
        }
    }
}
