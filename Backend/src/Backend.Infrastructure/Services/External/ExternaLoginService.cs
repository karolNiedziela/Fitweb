using Backend.Core.Entities;
using Backend.Infrastructure.Exceptions;
using Backend.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Infrastructure.Auth;
using Backend.Core.Factories;
using Backend.Infrastructure.DTO;

namespace Backend.Infrastructure.Services.External
{
    public class ExternaLoginService : IExternalLoginService
    {
        private readonly IFacebookAuthService _facebookAuthService;
        private readonly IUserRepository _userRepository;
        private readonly IJwtHandler _jwtHandler;
        private readonly IRefreshTokenFactory _refreshTokenFactory;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public ExternaLoginService(IFacebookAuthService facebookAuthService, IUserRepository userRepository,
            IJwtHandler jwtHandler, IRefreshTokenFactory refreshTokenFactory, IRefreshTokenRepository refreshTokenRepository)
        {
            _facebookAuthService = facebookAuthService;
            _userRepository = userRepository;
            _jwtHandler = jwtHandler;
            _refreshTokenFactory = refreshTokenFactory;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<JwtDto> LoginWithFacebookAsync(string accessToken)
        {
            var validatedTokenResult = await _facebookAuthService.ValidateAccessTokenAsync(accessToken);

            if (!validatedTokenResult.FacebookTokenValidationData.IsValid)
            {
                throw new ServiceException("Token_issue", "Invalid facebook token.");
            }

            var userInfo = await _facebookAuthService.GetUserInfoAsync(accessToken);

            var user = await _userRepository.GetByEmailAsync(userInfo.Email);
            if (user is null)
            {
                user = new User(userInfo.Email, userInfo.Email);

                user.UserRoles.Add(UserRole.Create(user, Role.GetRole("User")));

                await _userRepository.AddAsync(user);
            }

            var jwt = _jwtHandler.CreateToken(user.Id, user.UserName, Role.GetRole("User").Name);
            jwt.RefreshToken = await CreateRefreshTokenAsync(user.Id);

            return jwt;
        }

        private async Task<string> CreateRefreshTokenAsync(int userId)
        {
            var refreshToken = _refreshTokenFactory.Create(userId);
            await _refreshTokenRepository.AddAsync(refreshToken);

            return refreshToken.Token;
        }
    }
}
