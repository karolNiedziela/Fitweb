using Backend.Core.Helpers;
using Backend.Infrastructure.CommandQueryHandler.Queries;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers.Exercises
{
    public class GetExercisesHandler : IQueryHandler<GetExercises, PagedList<ExerciseDto>>
    {
        private readonly IExerciseService _exerciseService;

        public GetExercisesHandler(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        public async Task<PagedList<ExerciseDto>> HandleAsync(GetExercises query)
        {
            return await _exerciseService.GetAllAsync(new PaginationQuery
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            });
        }
    }
}
