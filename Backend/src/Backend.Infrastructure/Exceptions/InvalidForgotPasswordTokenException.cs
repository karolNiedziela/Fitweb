﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Exceptions
{
    public class InvalidForgotPasswordTokenException : InfrastructureException
    {
        public InvalidForgotPasswordTokenException() : base("Invalid forgot password token.")
        {
        }
    }
}
