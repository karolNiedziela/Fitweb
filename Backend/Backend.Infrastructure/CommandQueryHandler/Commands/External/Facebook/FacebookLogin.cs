using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Commands
{
    public class FacebookLogin : ICommand
    {
        public Guid TokenId { get; set; }

        public string AccessToken { get; set; }
    }
}
