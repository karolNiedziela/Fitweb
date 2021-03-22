namespace Backend.Core.Exceptions
{
    public class InvalidValueException : CoreException
    {
        public InvalidValueException(string value) : base($"{value} cannot be less than 0.")
        {
        }
    }

}
