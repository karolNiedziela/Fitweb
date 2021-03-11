using Backend.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend.Core.Entities
{
    public class CategoryOfProduct
    {
        public int Id { get; set; }

        public CategoryOfProductId Name { get; set; }

        public string CategoryName
        {
            get
            {
                return Name.ToString().Replace("_", " ");
            }
        }

        public CategoryOfProduct()
        {
        }

        public static CategoryOfProduct GetCategory(string categoryOfProductName)
        {
            categoryOfProductName = categoryOfProductName.Replace(" ", "_");
            return Enum.GetValues(typeof(CategoryOfProductId))
                             .Cast<CategoryOfProductId>()
                             .Select(cop => new CategoryOfProduct()
                             {
                                 Id = (int)cop,
                                 Name = cop
                             }).SingleOrDefault(cop => cop.Name.ToString() == categoryOfProductName);
        }
    }
}
