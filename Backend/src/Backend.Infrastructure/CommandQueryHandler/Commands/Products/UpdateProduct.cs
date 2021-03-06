﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.CommandQueryHandler.Commands
{
    public class UpdateProduct : ICommand
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public double Calories { get; set; }

        public double Proteins { get; set; }

        public double Carbohydrates { get; set; }

        public double Fats { get; set; }

        public string CategoryName { get; set; }
    }
}
