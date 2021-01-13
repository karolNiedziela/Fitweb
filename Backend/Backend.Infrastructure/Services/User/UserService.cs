using AutoMapper;
using Backend.Core.Entities;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Repositories;
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
        private readonly IPasswordHandler _passwordService;
        private readonly IAthleteRepository _athleteRepository;

        public UserService(IUserRepository userRepository, IMapper mapper, 
            IPasswordHandler passwordService, IAthleteRepository athleteRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordService = passwordService;
            _athleteRepository = athleteRepository;
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
            if( await _userRepository.AnyAsync(u => u.Username == username))
            {
                throw new ServiceException(ErrorCodes.UsernameInUse, $"User with '{username}' already exists.");
            }
            if (await _userRepository.AnyAsync(u => u.Email == email))
            {
                throw new ServiceException(ErrorCodes.UsernameInUse, $"User with '{email}' already exists.");
            }

            var hash = _passwordService.Hash(password);

            var user = new User(username, email, hash);

            roleName = await _userRepository.CheckIfAdminExists(roleName);

            user.UserRoles.Add(new UserRole
            {
                User = user,
                RoleId = Role.GetRole(roleName)
            });

            await _userRepository.AddAsync(user);

            await _athleteRepository.CheckIfAthleteAsync(user, roleName);

            return user.Id;
        }

        public async Task LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetAsync(username);
            if (user == null)
            {
                throw new ServiceException(ErrorCodes.InvalidCredentials, "Invalid credentials");
            }

            var isPasswordValid = _passwordService.IsValid(user.Password, password);
            if (!isPasswordValid)
            {
                throw new ServiceException(ErrorCodes.InvalidCredentials, "Invalid credentials");
            }      
        }

        public async Task DeleteAsync(int userId)
        {
            var user = await _userRepository.GetOrFailAsync(userId);
            
            await _userRepository.RemoveAsync(user);
        }
    
    }
}
