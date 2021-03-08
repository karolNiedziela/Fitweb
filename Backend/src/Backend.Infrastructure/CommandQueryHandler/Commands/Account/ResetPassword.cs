using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Commands
{
    public class ResetPassword : ICommand
    {
        public int UserId { get; set; }

        public string Code { get; set; }

        public string NewPassword { get; set; }

    }
}
