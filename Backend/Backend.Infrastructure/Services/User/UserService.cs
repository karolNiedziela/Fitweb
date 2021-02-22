using AutoMapper;
using Backend.Core.Entities;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Extensions;
using Backend.Core.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Infrastructure.Auth;

namespace Backend.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHandler _passwordHandler;

        public UserService(IMapper mapper, IPasswordHandler passwordHandler, IUserRepository userRepository)
        {
            _mapper = mapper;
            _passwordHandler = passwordHandler;
            _userRepository = userRepository;
        }

        public async Task<UserDto> GetAsync(int id)
        {
            var user = await _userRepository.GetAsync(id);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetAsync(string username)
        {
            var user = await _userRepository.GetAsync(username);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<int> RegisterAsync(string username, string email, string password, string roleName = "User")
        {
            if (await _userRepository.AnyAsync(u => u.Username == username))
            {
                throw new ServiceException(ErrorCodes.UsernameInUse, $"User with '{username}' already exists.");
            }

            if (await _userRepository.AnyAsync(u => u.Email == email))
            {
                throw new ServiceException(ErrorCodes.EmailInUse, $"User with '{email}' already exists.");
            }

            var hash = _passwordHandler.Hash(password);

            var user = new User(username, email, hash);

            user.UserRoles.Add(new UserRole
            {
                User = user,
                RoleId = Role.GetRole(roleName).Id
            });

            await _userRepository.AddAsync(user);
           
            return user.Id;
        }

        public async Task LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetAsync(username);
            if (user == null)
            {
                throw new ServiceException(ErrorCodes.InvalidCredentials, "Invalid credentials");
            }

            var isPasswordValid = _passwordHandler.IsValid(user.Password, password);
            if (!isPasswordValid)
            {
                throw new ServiceException(ErrorCodes.InvalidCredentials, "Invalid credentials");
            }
        }

        public async Task DeleteAsync(int userId)
        {
            var user = await _userRepository.GetOrFailAsync(userId);
            
            await _userRepository.DeleteAsync(user);
        }

        public async Task UpdateAsync(int id, string username, string email, string password)
        {
            var user = await _userRepository.GetOrFailAsync(id);
            var result = _userRepository.AnyAsync(u => u.Username == username);

            if (await _userRepository.AnyAsync(u => u.Username == username))
            {
                if (user.Username != username)
                {
                    throw new ServiceException(ErrorCodes.UsernameInUse, $"User with '{username}' already exists.");
                }
                
            }
            if (await _userRepository.AnyAsync(u => u.Email == email))
            {
                if (user.Email != email)
                {
                    throw new ServiceException(ErrorCodes.UsernameInUse, $"User with '{email}' already exists.");
                }            
            }

            user.SetUsername(username);
            user.SetEmail(email);
            var hash = _passwordHandler.Hash(password);
            user.SetPassword(hash);

            await _userRepository.UpdateAsync(user);
        }
    }
}
