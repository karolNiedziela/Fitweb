using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.DTO
{
    public class UserProductDto
    {
        public int Id { get; set; }
        public ProductDto Product { get; set; }
        public double Weight { get; set; }
        public double Calories { get; set; }
        public double Proteins { get; set; }
        public double Carbohydrates { get; set; }
        public double Fats { get; set; }

        public string AddedAt { get; set; }
    }
}
