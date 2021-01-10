using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.CommandHandler.Commands
{
    public class CreateAthlete : ICommand
    {
        public int UserId { get; set; }
    }
}
