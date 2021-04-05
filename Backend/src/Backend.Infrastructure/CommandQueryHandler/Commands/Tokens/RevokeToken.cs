using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Commands.Tokens
{
    public class RevokeToken : ICommand
    {
        public string RefreshToken { get; set; }
    }
}
