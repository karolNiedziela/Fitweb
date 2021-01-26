using Backend.Core.Factories;
using Backend.Core.Repositories;
using Backend.Infrastructure.Auth;
using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Services;
using Backend.Infrastructure.Services.External;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Handlers
{
    public class FacebookLoginHandler : ICommandHandler<FacebookLogin>
    {
        private readonly IUserService _userService;
        private readonly IExternalLoginService _externalLoginService;
        private readonly IJwtHandler _jwtHandler;
        private readonly IMemoryCache _cache;
        private readonly IRefreshTokenFactory _refreshTokenFactory;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public FacebookLoginHandler(IExternalLoginService externalLoginService, IJwtHandler jwtHandler,
        IMemoryCache cache, IRefreshTokenFactory refreshTokenFactory, IRefreshTokenRepository refreshTokenRepository,
        IUserService userService)
        {
            _externalLoginService = externalLoginService;
            _jwtHandler = jwtHandler;
            _cache = cache;
            _refreshTokenFactory = refreshTokenFactory;
            _refreshTokenRepository = refreshTokenRepository;
            _userService = userService;
        }

        public async Task HandleAsync(FacebookLogin command)
        {
            var email = await _externalLoginService.LoginWithFacebookAsync(command.AccessToken);
            var user = await _userService.GetAsync(email);
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
