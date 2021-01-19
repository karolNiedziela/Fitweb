using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.CommandQueryHandler.Commands
{
    public class AddExercise : ICommand
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PartOfBody { get; set; }
    }
}
