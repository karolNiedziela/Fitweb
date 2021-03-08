using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.CommandQueryHandler.Commands
{
    public class DeleteProduct : ICommand
    {
        public int ProductId { get; set; }
    }
}
