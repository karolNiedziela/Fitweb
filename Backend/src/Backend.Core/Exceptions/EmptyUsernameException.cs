using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Exceptions
{
    public class EmptyUsernameException : CoreException
    {
        public EmptyUsernameException() : base("Username cannot be empty.")
        {
        }
    }
}
