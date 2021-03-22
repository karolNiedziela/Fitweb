﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Exceptions
{
    public class InvalidUsernameException : CoreException
    {
        public InvalidUsernameException() : base($"Username must contain at least 6 characters " +
                    "and at most twenty characters.")
        {
        }
    }
}
