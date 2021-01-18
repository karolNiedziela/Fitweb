using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.CommandHandler.Commands
{
    public class UpdateExercise : ICommand
    {
        public int ExerciseId { get; set; }

        public string Name { get; set; }

        public string PartOfBody { get; set; }
    }
}
