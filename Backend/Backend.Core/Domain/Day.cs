using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Domain
{
    public class Day
    {
        public Days Id { get; set; }
        public string Name { get; set; }

        public Day()
        {


        }

        public Day(string name)
        {
            Name = name;
        }
    }
}
