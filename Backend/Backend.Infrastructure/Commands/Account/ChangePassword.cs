using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Commands.Account
{
    public class ChangePassword : AuthenticatedCommandBase
    {
        public string NewPassword { get; set; }
    }
}
