﻿using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Queries
{
    public class GetCategories : IQuery<IEnumerable<CategoryOfProduct>>
    {
    }
}
