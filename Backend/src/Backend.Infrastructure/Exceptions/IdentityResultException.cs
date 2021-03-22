using Backend.Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Exceptions
{
    public class IdentityResultException : InfrastructureException
    {
        public IdentityResultException(string message) : base(message)
        {
        }
    }
}
