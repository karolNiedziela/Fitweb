using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Queries.Athletes
{
    public class GetAthleteExercises : IQuery<AthleteDto>
    {
        public int AthleteId { get; set; }

        public string DayName { get; set; }

        public GetAthleteExercises(int athleteId, string dayName)
        {
            AthleteId = athleteId;
            DayName = dayName;
        }
    }
}
