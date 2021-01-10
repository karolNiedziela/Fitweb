﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.CommandHandler.Commands
{
    public class AuthenticatedCommandBase : IAuthenticatedCommand
    {
        public int UserId { get; set; }
    }
}