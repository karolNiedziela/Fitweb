using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.CommandQueryHandler.Commands
{
    public class DeleteUser : ICommand
    {
        public int Id { get; set; }
    }
}
