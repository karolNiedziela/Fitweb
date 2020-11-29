using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Commands.Exercises
{
    public class AddExercise : ICommand
    {
        public string PartOfBody { get; set; }
        public string Name { get; set; }
    }
}
