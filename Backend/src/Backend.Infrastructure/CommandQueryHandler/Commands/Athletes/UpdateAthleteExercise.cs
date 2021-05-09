using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Commands
{
    public class UpdateAthleteExercise : ICommand
    {
        public int UserId { get; set; }

        public int ExerciseId { get; set; }

        public double Weight { get; set; }

        public int NumberOfSets { get; set; }

        public int NumberOfReps { get; set; }

        public string DayName { get; set; }
    }
}
