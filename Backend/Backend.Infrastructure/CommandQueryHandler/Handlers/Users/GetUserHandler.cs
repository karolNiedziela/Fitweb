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
    public class GetUserHandler : IQueryHandler<GetUser, UserDto>
    {
        private readonly IUserService _userService;

        public GetUserHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserDto> HandleAsync(GetUser query)
        {
            return await _userService.GetAsync(query.Id); 
        }
    }
}
