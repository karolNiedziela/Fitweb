using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Settings
{
    public class AuthMessageSenderSettings
    {
        public string SendGridUser { get; set; }

        public string SendGridKey { get; set; }
    }
}
