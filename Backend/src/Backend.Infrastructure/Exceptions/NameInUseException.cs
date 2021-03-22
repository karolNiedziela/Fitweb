using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Exceptions
{
    public class NameInUseException : InfrastructureException
    {
        public NameInUseException(string what, string name) : base($"{what} with name: '{name}' already exists.")
        {
        }
    }
}
