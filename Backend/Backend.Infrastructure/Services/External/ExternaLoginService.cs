using Backend.Core.Entities;
using Backend.Infrastructure.Exceptions;
using Backend.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services.External
{
    public class ExternaLoginService : IExternalLoginService
    {
        private readonly IFacebookAuthService _facebookAuthService;
        private readonly IUserRepository _userRepository;

        public ExternaLoginService(IFacebookAuthService facebookAuthService, IUserRepository userRepository)
        {
            _facebookAuthService = facebookAuthService;
            _userRepository = userRepository;
        }

        public async Task<string> LoginWithFacebookAsync(string accessToken)
        {
            var validatedTokenResult = await _facebookAuthService.ValidateAccessTokenAsync(accessToken);

            if (!validatedTokenResult.FacebookTokenValidationData.IsValid)
            {
                throw new ServiceException("Token_issue", "Invalid facebook token");
            }

            var userInfo = await _facebookAuthService.GetUserInfoAsync(accessToken);

            var user = await _userRepository.GetAsync(userInfo.Email);
            if (user is null)
            {
                user = new User(userInfo.Email, userInfo.Email);

                user.UserRoles.Add(new UserRole
                {
                    User = user,
                    RoleId = Role.GetRole("User").Id
                });

                await _userRepository.AddAsync(user);
            }

            return userInfo.Email;
        }
    }
}
