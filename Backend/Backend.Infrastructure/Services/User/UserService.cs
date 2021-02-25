using AutoMapper;
using Backend.Core.Entities;
using Backend.Core.Factories;
using Backend.Core.Repositories;
using Backend.Infrastructure.Auth;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Extensions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<UserDto> GetByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task DeleteAsync(int userId)
        {
            var user = await _userRepository.GetOrFailAsync(userId);
            
            await _userRepository.DeleteAsync(user);
        }

        public async Task UpdateAsync(int id, string username, string email, string password)
        {
            var user = await _userRepository.GetOrFailAsync(id);

            user.SetUsername(username);
            user.SetEmail(email);
            var hash = _passwordHandler.Hash(password);
            user.SetPassword(hash);

            try
            {
                await _userRepository.UpdateAsync(user);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        } 

        private void HandleException(Exception exception)
        {
            if (exception.InnerException is SqlException sqlException)
            {
                if (sqlException.Message.Contains("IX_Users_Email"))
                {
                    throw new ServiceException(ErrorCodes.EmailInUse, "Email is already taken.");
                }
                else if (sqlException.Message.Contains("IX_Users_Username"))
                {
                    throw new ServiceException(ErrorCodes.UsernameInUse, "Username is already taken.");
                }
                else
                {
                    throw new ServiceException("Something went wrong.", "Exception during registration. Contact us example@email.com");
                }
            }
            else
            {
                throw new ServiceException("Something went wrong.", "Exception during registration. Contact us example@email.com");
            }
        }
    }
}
