using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.CommandQueryHandler.Commands
{
    public class CreateAthlete : ICommand
    {
        public int UserId { get; set; }
    }
}
