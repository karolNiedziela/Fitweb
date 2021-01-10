using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.CommandHandler.Commands
{
    public class DeleteProduct : ICommand
    {
        public int ProductId { get; set; }
    }
}
