using Backend.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.EF.Configurations
{
    public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.PartOfBody).IsRequired().HasColumnType("nvarchar(200)");
            builder.Property(x => x.Name).IsRequired().HasColumnType("nvarchar(200)");

            builder.HasData(
                new Exercise { Id = 1, PartOfBody = "Abs", Name = "Crunch" },
                new Exercise { Id = 2, PartOfBody = "Abs", Name = "Side Plank" },
                new Exercise { Id = 3, PartOfBody = "Abs", Name = "Resisted Crunch" },
                new Exercise { Id = 4, PartOfBody = "Abs", Name = "Trunk Rotation" },
                new Exercise { Id = 5, PartOfBody = "Back", Name = "Pull-up" },
                new Exercise { Id = 6, PartOfBody = "Back", Name = "Back fly" },
                new Exercise { Id = 7, PartOfBody = "Back", Name = "Lateral pulldown" },
                new Exercise { Id = 8, PartOfBody = "Chest", Name = "Parallel bar dips" },
                new Exercise { Id = 9, PartOfBody = "Chest", Name = "Incline bench press" },
                new Exercise { Id = 10, PartOfBody = "Legs", Name = "Jumping squat" },
                new Exercise { Id = 12, PartOfBody = "Legs", Name = "Squat" },
                new Exercise { Id = 13, PartOfBody = "Biceps", Name = "Biceps curl" },
                new Exercise { Id = 14, PartOfBody = "Biceps", Name = "Chin-up" },
                new Exercise { Id = 15, PartOfBody = "Biceps", Name = "Lying biceps curl" },
                new Exercise { Id = 16, PartOfBody = "Triceps", Name = "Triceps extension" },
                new Exercise { Id = 17, PartOfBody = "Triceps", Name = "Prone triceps extension" },
                new Exercise { Id = 18, PartOfBody = "Triceps", Name = "Kneeling triceps extension" }
            );
        }
    }
}
