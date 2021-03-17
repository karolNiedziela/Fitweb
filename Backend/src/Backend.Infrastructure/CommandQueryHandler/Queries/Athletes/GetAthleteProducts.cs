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
        public int UserId { get; set; }

        public DateTime? Date { get; set;}

        public GetAthleteProducts(int userId, DateTime? date)
        {
            UserId = userId;
            Date = date;
        }
    }
}
