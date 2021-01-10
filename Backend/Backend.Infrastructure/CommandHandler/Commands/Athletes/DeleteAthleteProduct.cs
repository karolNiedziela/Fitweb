using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.CommandHandler.Commands
{
    public class DeleteAthleteProduct : ICommand
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }
    }
}
