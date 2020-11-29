using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Commands.Account
{
    public class EditProfile : AuthenticatedCommandBase
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
