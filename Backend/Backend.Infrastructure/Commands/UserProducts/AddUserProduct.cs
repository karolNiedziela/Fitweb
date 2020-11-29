using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Commands.UserProducts
{
    public class AddUserProduct : AuthenticatedCommandBase
    {
        public int ProductId { get; set; }
        public double Weight { get; set; }
    }
}
