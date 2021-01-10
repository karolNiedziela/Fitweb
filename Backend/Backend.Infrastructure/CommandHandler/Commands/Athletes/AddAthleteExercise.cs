using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.CommandHandler.Commands
{
    public class AddAthleteExercise : ICommand
    {
        public int UserId { get; set; }

        public int ExerciseId { get; set; }

        public double Weight { get; set; }
       
        public int NumberOfSets { get; set; }
        
        public int NumberOfReps { get; set; }

        public string DayName { get; set; }
    }
}
