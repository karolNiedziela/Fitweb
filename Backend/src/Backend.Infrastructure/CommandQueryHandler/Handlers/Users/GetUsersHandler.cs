using Backend.Infrastructure.CommandQueryHandler.Queries.Users;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers.Users
{
    public class GetUsersHandler : IQueryHandler<GetUsers, IEnumerable<UserDto>>
    {
        private readonly IUserService _userService;

        public GetUsersHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IEnumerable<UserDto>> HandleAsync(GetUsers query)
        {
            return await _userService.GetAllAsync();
        }
    }
}
