using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Entities
{
    public class UserRole
    {
        public Role Role { get; set; }

        public int RoleId { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }

        public UserRole()
        {

        }
        public  UserRole(User user, Role role)
        {
            User = user;
            Role = role;
        }

        public static UserRole Create(User user, Role role)
            => new UserRole(user, role);
    }
}
