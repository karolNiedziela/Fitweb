using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Queries.Athletes
{
    public class GetAthlete : IQuery<AthleteDto>
    {
        public int UserId { get; set; }

        public GetAthlete(int userId)
        {
            UserId = userId;
        }
    }
}
