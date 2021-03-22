using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Exceptions
{
    public class ExerciseNotFoundException : InfrastructureException
    {
        public ExerciseNotFoundException(int exerciseId) : base($"Exercise with id: '{exerciseId}' was not found.")
        {
        }
    }
}
