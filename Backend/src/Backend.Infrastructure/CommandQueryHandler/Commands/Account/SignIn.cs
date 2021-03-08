using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.CommandQueryHandler.Commands
{
    public class SignIn : ICommand
    {
        public Guid TokenId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
