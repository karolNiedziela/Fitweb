using Backend.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Exceptions
{
    public class InvalidEmailTokenException : InfrastructureException
    {
        public InvalidEmailTokenException() : base("Invalid email confirmation token.")
        {
        }
    }
}
