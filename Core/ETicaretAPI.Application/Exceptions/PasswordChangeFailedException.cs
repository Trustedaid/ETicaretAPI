namespace ETicaretAPI.Application.Exceptions;

public class PasswordChangeFailedException: Exception
{
    public PasswordChangeFailedException(): base("Password update failed.")
    {
    }

    public PasswordChangeFailedException(string? message) : base(message)
    {
        
    }
    public PasswordChangeFailedException(string? message, Exception? innerException) : base(message, innerException)
    {
        
    }
}