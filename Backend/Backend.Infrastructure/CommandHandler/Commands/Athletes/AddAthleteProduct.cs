using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.CommandHandler.Commands
{
    public class AddAthleteProduct : ICommand
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }

        public double Weight { get; set; }
    }
}
