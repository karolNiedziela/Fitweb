using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.DTO
{
    public class AthleteDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public List<AthleteProductDto> Products { get; set; }

        public List<AthleteExerciseDto> Exercises { get; set; }

        public CaloricDemandDto CaloricDemand { get; set; }
    }
}
