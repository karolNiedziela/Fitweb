using Backend.Core.Helpers;
using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Queries
{
    public class GetProducts : IQuery<PagedList<ProductDetailsDto>>
    {
       public int PageNumber { get; set; }

       public int PageSize { get; set; }
    }
}
