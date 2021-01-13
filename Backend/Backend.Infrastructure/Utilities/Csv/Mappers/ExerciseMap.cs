using Backend.Core.Entities;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend.Infrastructure.Utilities.Csv
{
    public class ExerciseMap : ClassMap<Exercise>
    {
        public ExerciseMap()
        {
            Map(e => e.Name).Name("Name");
            Map(e => e.PartOfBodyId).Name("Part of body").ConvertUsing(row => Enum.GetValues(typeof(PartOfBodyId))
                            .Cast<PartOfBodyId>()
                            .Select(pob => new PartOfBody()
                            {
                                Id = (int)pob,
                                Name = pob
                            }).SingleOrDefault(pob => pob.Name.ToString() == row.GetField("Part of body")).Id);

            Map(e => e.Id).Ignore();
            Map(e => e.DateCreated).Ignore();
            Map(e => e.DateUpdated).Ignore();
        }
    }
}
