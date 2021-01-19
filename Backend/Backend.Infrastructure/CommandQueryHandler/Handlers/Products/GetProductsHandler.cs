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
    public class GetProductsHandler : IQueryHandler<GetProducts, PagedList<ProductDetailsDto>>
    {
        private readonly IProductService _productService;

        public GetProductsHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<PagedList<ProductDetailsDto>> HandleAsync(GetProducts query)
        {

            return await _productService.GetAllAsync(new PaginationQuery 
            { 
                PageSize = query.PageSize, 
                PageNumber = query.PageNumber 
            });
        }
    }
}
