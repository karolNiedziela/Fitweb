using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Exceptions
{
    public class AlreadyAddedTodayException : InfrastructureException
    {
        public AlreadyAddedTodayException(string what, int id) : base($"{what} with id: '{id}' already added today.")
        {
        }
    }  
}
