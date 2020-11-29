using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Commands.UserExercises
{
    public class AddUserExercise : AuthenticatedCommandBase
    {
        public int ExerciseId { get; set; }
        public double Weight { get; set; }
        public int NumberOfSets { get; set; }
        public int NumberOfReps { get; set; }
        public string Day { get; set; }
    }
}
