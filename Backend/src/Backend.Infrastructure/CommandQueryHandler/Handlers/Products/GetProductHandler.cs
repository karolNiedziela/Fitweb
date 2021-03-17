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
    public class GetProductHandler : IQueryHandler<GetProduct, ProductDto>
    {
        private readonly IProductService _productService;

        public GetProductHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ProductDto> HandleAsync(GetProduct query)
        {
            return await _productService.GetAsync(query.Id);
        }
    }
}
