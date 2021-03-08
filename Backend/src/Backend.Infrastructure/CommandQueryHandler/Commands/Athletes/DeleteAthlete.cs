using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.CommandQueryHandler.Commands.Athletes
{
    public class DeleteAthlete : ICommand
    {
        public int AthleteId { get; set; }
    }
}
