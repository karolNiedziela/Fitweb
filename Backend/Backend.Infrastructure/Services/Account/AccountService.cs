using Backend.Core.Entities;
using Backend.Core.Factories;
using Backend.Core.Repositories;
using Backend.Infrastructure.Auth;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHandler _passwordHandler;
        private readonly IJwtHandler _jwtHandler;
        private readonly IRefreshTokenFactory _refreshTokenFactory;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AccountService(IUserRepository userRepository, IPasswordHandler passwordHandler,
            IJwtHandler jwtHandler, IRefreshTokenFactory refreshTokenFactory, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _passwordHandler = passwordHandler;
            _jwtHandler = jwtHandler;
            _refreshTokenFactory = refreshTokenFactory;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<int> SignUpAsync(string username, string email, string password, string roleName = "User")
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user != null)
            {
                throw new ServiceException(ErrorCodes.UsernameInUse, "Username is already taken.");
            }

            user = await _userRepository.GetByEmailAsync(email);
            if (user != null)
            {
                throw new ServiceException(ErrorCodes.EmailInUse, "Email is already taken.");
            }

            var hash = _passwordHandler.Hash(password);

            user = new User(username, email, hash);

            user.UserRoles.Add(new UserRole
            {
                User = user,
                RoleId = Role.GetRole(roleName).Id
            });

            await _userRepository.AddAsync(user);

            return user.Id;
        }

        public async Task<JwtDto> SignInAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                throw new ServiceException(ErrorCodes.InvalidCredentials, "Invalid credentials");
            }

            var isPasswordValid = _passwordHandler.IsValid(user.Password, password);
            if (!isPasswordValid)
            {
                throw new ServiceException(ErrorCodes.InvalidCredentials, "Invalid credentials");
            }

            var jwt = _jwtHandler.CreateToken(user.Id, user.Username, user.UserRoles.Select(ur => ur.Role.Name.ToString()).FirstOrDefault());
            jwt.RefreshToken = await CreateRefreshTokenAsync(user.Id);

            return jwt;
        }

        public async Task ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetOrFailAsync(userId);
            if (user == null)
            {
                throw new ServiceException(ErrorCodes.UserNotFound, $"User with id: '{userId}' was not found.");
            }

            var oldPasswordHash = _passwordHandler.Hash(oldPassword);
            if (!_passwordHandler.IsValid(oldPasswordHash, oldPassword))
            {
                throw new ServiceException(ErrorCodes.InvalidValue, "Password is not valid.");
            }

            var hash = _passwordHandler.Hash(newPassword);
            user.SetPassword(hash);

            await _userRepository.UpdateAsync(user);
        }

        private async Task<string> CreateRefreshTokenAsync(int userId)
        {
            var refreshToken = _refreshTokenFactory.Create(userId);
            await _refreshTokenRepository.AddAsync(refreshToken);

            return refreshToken.Token;
        }
    }
}
