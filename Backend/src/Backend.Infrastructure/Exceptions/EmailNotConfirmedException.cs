using Backend.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Exceptions
{
    public class EmailNotConfirmedException : InfrastructureException
    {
        public EmailNotConfirmedException() : base("Email not confirmed. Confirm email to get access.")
        {
        }
    }
}
