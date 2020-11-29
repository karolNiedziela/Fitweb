using Backend.Infrastructure.Commands;
using Backend.Infrastructure.Commands.Users;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Handlers.Users
{
    public class LoginHandler : ICommandHandler<Login>
    {
        private readonly IUserService _userService;
        private readonly IJwtHandler _jwtHandler;
        private readonly IMemoryCache _cache;

        public LoginHandler(IUserService userService, IJwtHandler jwtHandler, IMemoryCache cache)
        {
            _userService = userService;
            _jwtHandler = jwtHandler;
            _cache = cache;
        }
        public async Task HandleAsync(Login command)
        {
            await _userService.LoginAsync(command.Username, command.Password);
            var user = await _userService.GetAsync(command.Username);

            var jwt = _jwtHandler.CreateToken(user.Id, user.Role);
            _cache.SetJwt(command.TokenId, jwt);
        }
    }
}
