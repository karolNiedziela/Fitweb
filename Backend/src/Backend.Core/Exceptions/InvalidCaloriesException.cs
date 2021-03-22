namespace Backend.Core.Exceptions
{
    public class InvalidCaloriesException : CoreException
    {
        public InvalidCaloriesException() : base("Calories cannot be less than 0.")
        {
        }
    }
}