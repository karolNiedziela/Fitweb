using Backend.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Exceptions
{
    public class InvalidCredentialsException : InfrastructureException
    {
        public InvalidCredentialsException() : base("Invalid credentials.")
        {
        }
    }
}
