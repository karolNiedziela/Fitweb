using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.CommandQueryHandler.Commands
{
    public class DeleteExercise : ICommand
    {
        public int ExerciseId { get; set; }
    }
}
