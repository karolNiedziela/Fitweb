using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.CommandHandler.Commands
{
    public interface IAuthenticatedCommand : ICommand
    {
        int UserId { get; set; }
    }
}
