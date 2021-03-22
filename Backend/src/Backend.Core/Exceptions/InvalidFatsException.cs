namespace Backend.Core.Exceptions
{
    public class InvalidFatsException : CoreException
    {
        public InvalidFatsException() : base("Fats cannot be less than 0.")
        {
        }
    }
}
