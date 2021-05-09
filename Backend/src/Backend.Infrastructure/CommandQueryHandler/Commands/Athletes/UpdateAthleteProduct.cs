using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Commands
{
    public class UpdateAthleteProduct : ICommand
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int AthleteProductId { get; set; }

        public double Weight { get; set; }

    }
}
