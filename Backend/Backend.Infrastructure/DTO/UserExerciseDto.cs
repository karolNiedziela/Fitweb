using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.DTO
{
    public class UserExerciseDto
    {
        public int Id { get; set; }
        public ExerciseDto Exercise { get; set; }
        public double Weight { get; set; }
        public int NumberOfSets { get; set; }
        public int NumberOfReps { get; set; }
        public string Day { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
