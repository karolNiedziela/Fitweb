using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend.Core.Entities
{
    public class Role : IdentityRole<int>
    {

        public override string Name { get; set; }

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
                                Name = r.ToString()
                            }).SingleOrDefault(r => r.Name.ToString() == roleName);
    }
}
