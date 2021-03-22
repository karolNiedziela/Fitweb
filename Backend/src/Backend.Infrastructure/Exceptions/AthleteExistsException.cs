using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Exceptions
{
    public class AthleteExistsException : InfrastructureException
    {
        public AthleteExistsException(int userId) : base($"Athlete with user id: '{userId}' already exists.")
        {
        }
    }
}
