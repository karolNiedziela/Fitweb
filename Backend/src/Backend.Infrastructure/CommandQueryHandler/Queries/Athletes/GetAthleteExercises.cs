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
        public int UserId { get; set; }

        public string DayName { get; set; }

        public GetAthleteExercises(int userId, string dayName)
        {
            UserId = userId;
            DayName = dayName;
        }
    }
}
