namespace ETicaretAPI.Application.Exceptions;

public class UserCreateInvalidException : Exception
{
    public UserCreateInvalidException() : base("Unexpected error occurred.")
    {
        
    }

    public UserCreateInvalidException(string? message) : base(message)
    {
        
    }
    
    public UserCreateInvalidException(string? message, Exception? innerException) : base(message, innerException)
    {
        
    }
}