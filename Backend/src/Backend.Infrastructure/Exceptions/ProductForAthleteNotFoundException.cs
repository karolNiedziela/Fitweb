using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Exceptions
{
    public class ProductForAthleteNotFoundException : InfrastructureException
    {
        public ProductForAthleteNotFoundException(int userId, int productId) : base($"Product with id: '{productId}' " +
                    $"for athlete with user id: '{userId}' was not found.")
        {
        }
    }
}
