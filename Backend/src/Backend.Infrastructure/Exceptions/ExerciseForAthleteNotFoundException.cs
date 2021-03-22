using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Exceptions
{
    public class ExerciseForAthleteNotFoundException : InfrastructureException
    {
        public ExerciseForAthleteNotFoundException(int userId, int exerciseId) : base($"Exercise with id: '{exerciseId}' " +
                    $"for athlete with user id: '{userId}' was not found.")
        {
        }
    }
}
