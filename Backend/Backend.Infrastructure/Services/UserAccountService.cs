using Backend.Core.Repositories;
using Backend.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncrypter _encrypter;
        
        public UserAccountService(IUserRepository userRepository, IEncrypter encrypter)
        {
            _userRepository = userRepository;
            _encrypter = encrypter;
        }

        public async Task ChangePasswordAsync(int userId, string newPassword)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new ServiceException(ErrorCodes.UserNotFound, $"User with {user.Id} does not exist.");
            }

            var hash = _encrypter.GetHash(newPassword, user.Salt);
            if (hash == user.Password)
            {
                return;
            }

            var salt = _encrypter.GetSalt(newPassword);
            hash = _encrypter.GetHash(newPassword, salt);

            user.SetPassword(hash, salt);
            await _userRepository.UpdateAsync(user);
        }

        public async Task EditProfile(int userId, string username, string email)
        {
            var user = await _userRepository.GetAsync(userId);

            if (user.Username == username && user.Email == email)
                return;
            else if (user.Username == username)
            {
                var user2 = await _userRepository.GetAsync(email);
                if (user2 != null)
                {
                    throw new ServiceException(ErrorCodes.EmailInUse, $"{user2.Email} is already used.");
                }

                user.SetEmail(email);
                await _userRepository.UpdateAsync(user);
                return;
            }
            else if (user.Email == email)
            {
                var user2 = await _userRepository.GetAsync(username);
                if (user2 != null)
                {
                    throw new ServiceException(ErrorCodes.UsernameInUse, $"{user2.Username} is already used.");
                }

                user.SetUsername(username);
                await _userRepository.UpdateAsync(user);
                return;
            }    
            else
            {
                throw new ServiceException(ErrorCodes.UsernameInUse, $"This data is already used.");
            }
        }
    }
}
