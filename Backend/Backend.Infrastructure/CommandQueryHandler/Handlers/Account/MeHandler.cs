using Backend.Infrastructure.CommandQueryHandler.Queries;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers.Account
{
    public class MeHandler : IQueryHandler<Me, UserDto>
    {
        private readonly IUserService _userService;

        public MeHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<UserDto> HandleAsync(Me query)
        {
            return await _userService.GetAsync(query.UserId);
        }
    }
}
