using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Exceptions
{
    public class InvalidNumberOfSetsException : CoreException
    {
        public InvalidNumberOfSetsException() : base($"Number of sets cannot be less than or equal to 0.")
        {
        }
    }
}
