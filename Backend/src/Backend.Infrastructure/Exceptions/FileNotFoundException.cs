using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Exceptions
{
    public class FileNotFoundException : InfrastructureException
    {
        public FileNotFoundException() : base($"No file provided.")
        {
        }
    }
}
