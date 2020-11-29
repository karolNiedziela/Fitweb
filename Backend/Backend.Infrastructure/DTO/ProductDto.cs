using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.DTO
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Calories { get; set; }
        public double Proteins { get; set; }
        public double Carbohydrates { get; set; }
        public double Fats { get; set; }
    }
}
