using Backend.Core.Entities;
using Backend.Core.Enums;
using Backend.Core.Factories;
using Backend.Core.Repositories;
using Backend.Infrastructure.Auth;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services.External
{
    public class ExternaLoginService : IExternalLoginService
    {
        private readonly IFacebookAuthService _facebookAuthService;
        private readonly IUserRepository _userRepository;
        private readonly IJwtHandler _jwtHandler;
        private readonly IRefreshTokenFactory _refreshTokenFactory;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly UserManager<User> _userManager;
        private readonly IAthleteService _athleteService;

        public ExternaLoginService(IFacebookAuthService facebookAuthService, IUserRepository userRepository,
            IJwtHandler jwtHandler, IRefreshTokenFactory refreshTokenFactory, IRefreshTokenRepository refreshTokenRepository,
            UserManager<User> userManager, IAthleteService athleteService)
        {
            _facebookAuthService = facebookAuthService;
            _userRepository = userRepository;
            _jwtHandler = jwtHandler;
            _refreshTokenFactory = refreshTokenFactory;
            _refreshTokenRepository = refreshTokenRepository;
            _userManager = userManager;
            _athleteService = athleteService;
        }

        public async Task<JwtDto> LoginWithFacebookAsync(string accessToken)
        {
            var validatedTokenResult = await _facebookAuthService.ValidateAccessTokenAsync(accessToken);

            if (!validatedTokenResult.FacebookTokenValidationData.IsValid)
            {
                throw new InvalidFacebookTokenException();
            }

            var userInfo = await _facebookAuthService.GetUserInfoAsync(accessToken);

            var user = await _userRepository.GetByEmailAsync(userInfo.Email);

            // if user does not exist create user and sign in
            // else user exists, just sign in
            if (user is null)
            {
                user = new User(userInfo.Email, userInfo.Email);

                await _userManager.CreateAsync(user);

                await _userManager.AddToRoleAsync(user, RoleId.User.ToString());

                await _athleteService.CreateAsync(user.Id);
            }

            var jwt = _jwtHandler.CreateToken(user.Id, user.UserName, RoleId.User.ToString());
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
