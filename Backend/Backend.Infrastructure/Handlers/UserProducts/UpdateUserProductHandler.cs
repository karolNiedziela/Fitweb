using Backend.Infrastructure.Commands;
using Backend.Infrastructure.Commands.UserProducts;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Handlers.UserProducts
{
    public class UpdateUserProductHandler : ICommandHandler<UpdateUserProduct>
    {
        private readonly IUserProductService _userProductService;

        public UpdateUserProductHandler(IUserProductService userProductService)
        {
            _userProductService = userProductService;
        }

        public async Task HandleAsync(UpdateUserProduct command)
        {
            await _userProductService.UpdateAsync(command.UserId, command.ProductId, command.Weight);
        }
    }
}
