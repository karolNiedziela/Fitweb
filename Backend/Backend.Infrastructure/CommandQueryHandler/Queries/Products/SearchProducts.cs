using Backend.Core.Helpers;
using Backend.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Queries
{
    public class SearchProducts : IQuery<PagedList<ProductDetailsDto>>
    {
        public string Name { get; set; }

        public string Category { get; set; } = null;

        public PaginationQuery PaginationQuery { get; set; }
    }
}
