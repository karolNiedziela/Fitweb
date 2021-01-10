using Backend.Infrastructure.CommandHandler.Commands;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandHandler.Handlers
{
    public class AddProductHandler : ICommandHandler<AddProduct>
    {
        private readonly IProductService _productService;

        public AddProductHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task HandleAsync(AddProduct command)
        {
            command.Id = await _productService.AddAsync(command.Name, command.Calories, command.Proteins, command.Carbohydrates, command.Fats,
                command.CategoryName);
        }
    }
}
