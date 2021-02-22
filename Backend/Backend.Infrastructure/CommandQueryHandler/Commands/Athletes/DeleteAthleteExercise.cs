using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.CommandQueryHandler.Commands
{
    public class DeleteAthleteExercise : ICommand
    {
        public int AthleteId { get; set; }

        public int ExerciseId { get; set; }
    }
}
