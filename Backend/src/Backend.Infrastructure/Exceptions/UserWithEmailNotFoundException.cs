using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Exceptions
{
    public class UserWithEmailNotFoundException : InfrastructureException
    {
        public UserWithEmailNotFoundException(string email) : base($"User with '{email}' was not found.")
        {
        }
    }
}
