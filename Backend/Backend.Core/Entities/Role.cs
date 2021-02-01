using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend.Core.Entities
{
    public class Role
    {
        public int Id { get; set; }

        public RoleId Name { get; set; }

        public List<UserRole> UserRoles { get; set; }

        public Role()
        {

        }

        public static Role GetRole(string roleName)
            =>  Enum.GetValues(typeof(RoleId))
                            .Cast<RoleId>()
                            .Select(r => new Role()
                            {
                                Id = (int)r,
                                Name = r
                            }).SingleOrDefault(r => r.Name.ToString() == roleName);
    }
}
