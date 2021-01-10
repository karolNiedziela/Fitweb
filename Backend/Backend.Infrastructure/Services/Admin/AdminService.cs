using Backend.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;

        public AdminService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> CheckIfAdminExists()
        {
            var users = await _userRepository.GetAllAsync();
            if (users.Any(u => u.UserRoles.Any(ur => ur.Role.Name.ToString() == "Admin")))
            {
                return true;
            }

            return false;
        }
    }
}
