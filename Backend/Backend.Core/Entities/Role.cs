using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Entities
{
    public class Role
    {
        public int Id { get; set; }

        public RoleId Name { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

        public Role()
        {

        }
    }
}
