using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Queries.Athletes
{
    public class GetAthleteProducts : IQuery<AthleteDto>
    {
        public int AthleteId { get; set; }

        public DateTime? Date { get; set;}

        public GetAthleteProducts(int athleteId, DateTime? date)
        {
            AthleteId = athleteId;
            Date = date;
        }
    }
}
