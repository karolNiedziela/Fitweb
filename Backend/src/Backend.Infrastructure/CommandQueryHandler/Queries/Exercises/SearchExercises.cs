using Backend.Core.Helpers;
using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Queries
{
    public class SearchExercises : IQuery<PagedList<ExerciseDto>>
    {
        public string Name { get; set; }

        public string PartOfBody { get; set; } = null;

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
