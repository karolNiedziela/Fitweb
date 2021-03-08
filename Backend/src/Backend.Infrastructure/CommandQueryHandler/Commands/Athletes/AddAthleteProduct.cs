using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.CommandQueryHandler.Commands
{
    public class AddAthleteProduct : ICommand
    {
        public int AthleteId { get; set; }

        public int ProductId { get; set; }

        public double Weight { get; set; }
    }
}
