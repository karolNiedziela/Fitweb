using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Queries
{
    public class GetProduct : IQuery<ProductDetailsDto>
    { 
        public int Id { get;  }

        public GetProduct(int id)
        {
            Id = id;
        }
    }
}
