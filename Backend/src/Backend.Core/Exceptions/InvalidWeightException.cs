

namespace Backend.Core.Exceptions
{
    public class InvalidWeightException : CoreException
    {
        public InvalidWeightException() : base("Weight cannot be less than or equal to 0.")
        {
        }
    }
}
