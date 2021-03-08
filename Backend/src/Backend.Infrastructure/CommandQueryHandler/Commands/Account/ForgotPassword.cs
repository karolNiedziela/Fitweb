using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Commands
{
    public class ForgotPassword : ICommand
    {
        public string Email { get; set; }
    }
}
