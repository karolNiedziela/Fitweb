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

        public UserService(IMapper mapper,  IUserRepository userRepository)
        {
            _mapper = mapper;
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
     
    }
}
