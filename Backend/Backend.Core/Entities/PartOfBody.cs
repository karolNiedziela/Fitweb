using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend.Core.Entities
{
    public class PartOfBody
    {
        public int Id { get; set; }

        public PartOfBodyId Name { get; set; }

        public PartOfBody()
        {

        }

        public static PartOfBody GetPart(string partOfBody)
            => Enum.GetValues(typeof(PartOfBodyId))
               .Cast<PartOfBodyId>()
               .Select(pob => new PartOfBody
               {
                    Id = (int)pob,
                    Name = pob
               }).SingleOrDefault(pob => pob.Name.ToString() == partOfBody);

    }
}
