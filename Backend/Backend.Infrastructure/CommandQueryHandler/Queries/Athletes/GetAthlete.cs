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
        public int Id { get; set; }

        public GetAthlete(int id)
        {
            Id = id;
        }
    }
}
