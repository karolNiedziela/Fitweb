using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Domain
{
    public class Role
    {
        public Roles Id { get; set; }
        public string Name { get; set; }

        public Role()
        {


        }
        
        public Role(string name)
        {
            Name = name;
        }

    }
}
