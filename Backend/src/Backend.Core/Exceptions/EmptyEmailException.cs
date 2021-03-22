using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Exceptions
{
    public class EmptyEmailException : CoreException
    {
        public EmptyEmailException() : base("Email cannot be empty.")
        {
        }
    }

}
