using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend.Core.Entities
{
    public class Role : IdentityRole<int>
    {
        public List<UserRole> UserRoles { get; set; }

        public Role()
        {

        }

        public Role(string role)
        {
            Name = role;
            NormalizedName = role.ToUpper();
        }
    }
}
