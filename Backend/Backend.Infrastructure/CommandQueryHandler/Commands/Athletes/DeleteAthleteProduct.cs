using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.CommandQueryHandler.Commands
{
    public class DeleteAthleteProduct : ICommand
    {
        public int AthleteId { get; set; }

        public int ProductId { get; set; }
    }
}
