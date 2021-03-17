using Backend.Core.Helpers;
using Backend.Infrastructure.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Queries
{
    public class GetExercises : IQuery<PagedList<ExerciseDto>>
    {
        public string Name { get; set; } 

        public string PartOfBody { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

    }
}
