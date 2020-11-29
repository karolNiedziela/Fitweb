﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Commands.Users
{
    public class Login : ICommand
    {
        public Guid TokenId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
