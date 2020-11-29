using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Commands.Users
{
    public class CreateUser : ICommand
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
