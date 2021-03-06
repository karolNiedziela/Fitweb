﻿using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Queries.Athletes
{
    public class GetAthleteExercise : IQuery<AthleteDto>
    {
        public int UserId { get; set; }

        public int ExerciseId { get; set; }
    
        public GetAthleteExercise(int userId, int exerciseId)
        {
            UserId = userId;
            ExerciseId = exerciseId;
        }
    }
}
