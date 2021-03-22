namespace Backend.Infrastructure.Exceptions
{
    public class ProductNotFoundException : InfrastructureException
    {
        public ProductNotFoundException(int productId) : base($"Product with id: '{productId}' was not found.")
        {
        }
    }
}
