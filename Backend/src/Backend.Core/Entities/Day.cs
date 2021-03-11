using Backend.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend.Core.Entities
{
    // using solution https://stackoverflow.com/questions/50375357/how-to-create-a-table-corresponding-to-enum-in-ef-core-code-first

    public class Day
    {
        public int Id { get; set; }

        public DayId Name { get; set; }

        public Day()
        {
        }

        public static Day GetDay(string dayName)
            => Enum.GetValues(typeof(DayId))
                   .Cast<DayId>()
                   .Select(d => new Day()
                   {
                        Id = (int)d,
                        Name = d
                    }).SingleOrDefault(r => r.Name.ToString() == dayName);      
    }
}
