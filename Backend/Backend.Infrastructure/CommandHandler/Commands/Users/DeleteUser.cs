using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.CommandHandler.Commands
{
    public class DeleteUser : ICommand
    {
        public int UserId { get; set; }
    }
}
