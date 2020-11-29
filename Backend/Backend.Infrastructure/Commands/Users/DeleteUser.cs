using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Commands.Users
{
    public class DeleteUser : ICommand
    {
        public int UserId { get; set; }
    }
}
