using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Exceptions
{
    public class InvalidNumberOfRepsException : CoreException
    {
        public InvalidNumberOfRepsException() : base($"Number of reps cannot be less than or equal to 0.")
        {
        }
    }
}
