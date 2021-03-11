using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Entities
{
    public class UserRole : IdentityUserRole<int>
    {
        public Role Role { get; set; }

        public User User { get; set; }


        public UserRole()
        {

        }
        public  UserRole(User user, Role role)
        {
            User = user;
            RoleId = role.Id;
        }

        public static UserRole Create(User user, Role role)
            => new UserRole(user, role);
    }
}
