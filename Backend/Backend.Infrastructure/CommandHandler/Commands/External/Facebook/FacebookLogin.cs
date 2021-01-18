using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandHandler.Commands.External
{
    public class FacebookLogin : ICommand
    {
        public Guid TokenId { get; set; }

        public string AccessToken { get; set; }
    }
}
