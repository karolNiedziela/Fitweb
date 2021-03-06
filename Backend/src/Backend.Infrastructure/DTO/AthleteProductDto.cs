﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.DTO
{
    public class AthleteProductDto
    { 
        public int Id { get; set; }

        public ProductDto Product { get; set; }

        public double Weight { get; set; }

        public string DateCreated { get; set; }

        public string DateUpdated { get; set; }

    }
}
