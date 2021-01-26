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
    public class SearchExercisesHandler : IQueryHandler<SearchExercises, PagedList<ExerciseDto>>
    {
        private readonly IExerciseService _exerciseService;

        public SearchExercisesHandler(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        public async Task<PagedList<ExerciseDto>> HandleAsync(SearchExercises query)
        {
            return await _exerciseService.SearchAsync(new PaginationQuery
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            }, query.Name, query.PartOfBody);
        }
    }
}
