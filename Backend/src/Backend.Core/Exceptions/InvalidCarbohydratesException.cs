namespace Backend.Core.Exceptions
{
    public class InvalidCarbohydratesException : CoreException
    {
        public InvalidCarbohydratesException() : base("Carbohydrates cannot be less than 0.")
        {
        }
    }

}