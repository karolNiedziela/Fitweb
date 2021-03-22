using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Exceptions
{
    public class InvalidTotalCaloriesException : CoreException
    {
        public InvalidTotalCaloriesException() : base($"Total calories cannot be less than or equal to 0.")
        {
        }
    }
}
