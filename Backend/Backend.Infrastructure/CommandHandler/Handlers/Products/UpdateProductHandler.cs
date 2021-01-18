using Backend.Infrastructure.CommandHandler.Commands;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandHandler.Handlers
{
    public class UpdateProductHandler : ICommandHandler<UpdateProduct>
    {
        private readonly IProductService _productService;
        
        public UpdateProductHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task HandleAsync(UpdateProduct command)
        {
           await _productService.UpdateAsync(command.ProductId, command.Name, command.Calories, command.Proteins, 
                command.Carbohydrates, command.Fats, command.CategoryName);
        }
    }
}
