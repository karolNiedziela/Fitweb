using Backend.Core.Helpers;
using Backend.Infrastructure.CommandQueryHandler.Queries;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers.Products
{
    public class SearchProductsHandler : IQueryHandler<SearchProducts, PagedList<ProductDetailsDto>>
    {
        private readonly IProductService _productService;

        public SearchProductsHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<PagedList<ProductDetailsDto>> HandleAsync(SearchProducts query)
        {
            return await _productService.SearchAsync(new PaginationQuery
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            }, query.Name, query.Category);
        }
    }
}
