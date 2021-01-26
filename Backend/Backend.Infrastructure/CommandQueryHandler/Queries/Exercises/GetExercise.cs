using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Queries.Exercises
{
    public class GetExercise : IQuery<ExerciseDto>
    {
        public int Id { get; set; }

        public GetExercise(int id)
        {
            Id = id;
        }
    }
}
