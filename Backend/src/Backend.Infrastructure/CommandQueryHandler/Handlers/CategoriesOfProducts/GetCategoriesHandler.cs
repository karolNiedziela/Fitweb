using Backend.Core.Entities;
using Backend.Infrastructure.CommandQueryHandler.Queries;
using Backend.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers.CategoriesOfProducts
{
    public class GetCategoriesHandler : IQueryHandler<GetCategories, IEnumerable<CategoryOfProduct>>
    {
        private readonly FitwebContext _context;

        public GetCategoriesHandler(FitwebContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryOfProduct>> HandleAsync(GetCategories query)
        {
            var categories = await _context.CategoriesOfProduct.OrderBy(cop => cop.Name).ToListAsync();

            return categories;
        }
    }
}
