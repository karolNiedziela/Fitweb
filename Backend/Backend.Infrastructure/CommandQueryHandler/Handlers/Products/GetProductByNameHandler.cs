using Backend.Infrastructure.CommandQueryHandler.Queries.Products;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers.Products
{
    public class GetProductByNameHandler : IQueryHandler<GetProductByName, ProductDetailsDto>
    {
        private readonly IProductService _productService;

        public GetProductByNameHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ProductDetailsDto> HandleAsync(GetProductByName query)
        {
            return await _productService.GetAsync(query.Name);
        }
    }
}
