using Backend.Core.Factories;
using Backend.Core.Repositories;
using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers
{
    public class LoginHandler : ICommandHandler<Login>
    {
        private readonly IUserService _userService;
        private readonly IJwtHandler _jwtHandler;
        private readonly IMemoryCache _cache;
        private readonly IRefreshTokenFactory _refreshTokenFactory;
        private readonly IRefreshTokenRepository _refreshTokenRepository;


        public LoginHandler(IUserService userService, IJwtHandler jwtHandler, IMemoryCache cache,
            IRefreshTokenFactory refreshTokenFactory, IRefreshTokenRepository refreshTokenRepository)
        {
            _userService = userService;
            _jwtHandler = jwtHandler;
            _cache = cache;
            _refreshTokenFactory = refreshTokenFactory;
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task HandleAsync(Login command)
        {
            await _userService.LoginAsync(command.Username, command.Password);
            var user = await _userService.GetAsync(command.Username);
            var jwt = _jwtHandler.CreateToken(user.Id, user.Username, user.Role);
            jwt.RefreshToken = await CreateRefreshTokenAsync(user.Id);
            _cache.SetJwt(command.TokenId, jwt);
        }

        private async Task<string> CreateRefreshTokenAsync(int userId)
        {
            var refreshToken = _refreshTokenFactory.Create(userId);
            await _refreshTokenRepository.AddAsync(refreshToken);

            return refreshToken.Token;
        }
    }
}
