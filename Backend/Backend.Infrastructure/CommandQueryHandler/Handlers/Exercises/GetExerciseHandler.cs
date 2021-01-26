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
    public class GetExerciseHandler : IQueryHandler<GetExercise, ExerciseDto>
    {
        private readonly IExerciseService _exerciseService;

        public GetExerciseHandler(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        public async Task<ExerciseDto> HandleAsync(GetExercise query)
        {
            return await _exerciseService.GetAsync(query.Id);
        }
    }
}
