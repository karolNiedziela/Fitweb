using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Queries.Products
{
    public class GetProductByName : IQuery<ProductDto>
    {
        public string Name { get; set; }

        public GetProductByName(string name)
        {
            Name = name;
        }
    }
}
