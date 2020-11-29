using AutoMapper;
using Backend.Core.Domain;
using Backend.Core.Repositories;
using Backend.Infrastructure.DTO;
using Backend.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEncrypter _encrypter;
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository, IMapper mapper, IEncrypter encrypter, 
            IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _encrypter = encrypter;
            _roleRepository = roleRepository;
        }

        public async Task<UserDetailsDto> GetAsync(int id)
        {
            var user = await _userRepository.GetAsync(id);

            return _mapper.Map<UserDetailsDto>(user);
        }

        public async Task<UserDetailsDto> GetAsync(string username)
        {
            var user = await _userRepository.GetAsync(username);

            return _mapper.Map<UserDetailsDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task RegisterAsync(string username, string email, string password, string roleName)
        {
            var user = await _userRepository.GetAsync(username);
            if (user != null)
            {
                throw new ServiceException(ErrorCodes.UsernameInUse ,$"User with '{user.Username}' already exists.");
            }
            user = await _userRepository.GetAsync(email);
            if (user != null)
            {
                throw new ServiceException(ErrorCodes.EmailInUse, $"User with '{user.Email}' already exists.");
            }

            var role = await _roleRepository.GetAsync(roleName);

            var salt = _encrypter.GetSalt(password);
            var hash = _encrypter.GetHash(password, salt);
            user = new User(username, email, hash, salt, role);
            await _userRepository.AddAsync(user);
        }

        public async Task LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetAsync(username);
            if (user == null)
            {
                throw new ServiceException(ErrorCodes.InvalidCredentials, "Invalid credentials");
            }

            var hash = _encrypter.GetHash(password, user.Salt);
            if (user.Password == hash)
            {
                return;
            }

            throw new ServiceException(ErrorCodes.InvalidCredentials, "Invalid credentials");
        }

        public async Task DeleteAsync(int userId)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new ServiceException(ErrorCodes.InvalidCredentials, "Invalid credentials");
            }

            await _userRepository.RemoveAsync(user);
        }

        public async Task DeleteAsync(string username)
        {
            var user = await _userRepository.GetAsync(username);
            if (user == null)
            {
                throw new ServiceException(ErrorCodes.InvalidCredentials, "Invalid credentials");
            }

            await _userRepository.RemoveAsync(user);
        }
    }
}
