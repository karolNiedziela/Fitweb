using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Queries.Athletes
{
    public class GetAthleteProduct : IQuery<AthleteDto>
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }
    
        public GetAthleteProduct(int userId, int productId)
        {
            UserId = userId;
            ProductId = productId;
        }
    }
}
