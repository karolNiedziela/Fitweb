using Backend.Infrastructure.Commands;
using Backend.Infrastructure.Commands.UserProducts;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Handlers.UserProducts
{
    public class AddUserProductHandler : ICommandHandler<AddUserProduct>
    {
        private readonly IUserProductService _userProductService;

        public AddUserProductHandler(IUserProductService userProductService)
        {
            _userProductService = userProductService;
        }

        public async Task HandleAsync(AddUserProduct command)
        {
            await _userProductService.AddAsync(command.UserId, command.ProductId, command.Weight);
        }
    }
}
