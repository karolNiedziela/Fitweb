using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Queries.Exercises
{
    public class GetExerciseByName : IQuery<ExerciseDto>
    {
        public string Name { get; set; }

        public GetExerciseByName(string name)
        {
            Name = name;
        }
    }
}
