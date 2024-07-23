namespace ETicaretAPI.Application.Exceptions;

public class AuthenticationFailException: Exception
{
    public AuthenticationFailException() : base("Authentication failed")
    {
        
    }

    public AuthenticationFailException(string message) : base(message)
    {
        
    }
    
    public AuthenticationFailException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}