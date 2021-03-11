using Backend.Core.Entities;
using Backend.Core.Enums;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Backend.Infrastructure.Utilities.Csv
{
    public class ProductMap : ClassMap<Product>
    {
        public ProductMap()
        {
            Map(p => p.Name).Name("Name");
            Map(p => p.Calories).Name("Calories");
            Map(p => p.Proteins).Name("Proteins");
            Map(p => p.Carbohydrates).Name("Carbohydrates");
            Map(p => p.Fats).Name("Fats");
            Map(p => p.CategoryOfProductId).Name("Category").ConvertUsing(row => Enum.GetValues(typeof(CategoryOfProductId))
                           .Cast<CategoryOfProductId>()
                           .Select(cop => new CategoryOfProduct()
                           {
                               Id = (int)cop,
                               Name = cop
                           }).SingleOrDefault(cop => cop.Name.ToString() == row.GetField("Category")).Id);

            Map(p => p.Id).Ignore();
            Map(p => p.DateCreated).Ignore();
            Map(p => p.DateUpdated).Ignore();
            Map(p => p.AthleteProducts).Ignore();
        }
    }
}
