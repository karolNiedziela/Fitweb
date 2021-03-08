using Backend.Infrastructure.CommandQueryHandler.Queries.Exercises;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers.Exercises
{
    public class GetExerciseByNameHandler : IQueryHandler<GetExerciseByName, ExerciseDto>
    {
        private readonly IExerciseService _exerciseService;

        public GetExerciseByNameHandler(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        public async Task<ExerciseDto> HandleAsync(GetExerciseByName query)
        {
            return await _exerciseService.GetAsync(query.Name);
        }
    }
}
