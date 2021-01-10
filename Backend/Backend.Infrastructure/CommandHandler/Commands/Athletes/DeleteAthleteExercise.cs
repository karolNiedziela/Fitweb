using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.CommandHandler.Commands
{
    public class DeleteAthleteExercise : ICommand
    {
        public int UserId { get; set; }

        public int ExerciseId { get; set; }
    }
}
