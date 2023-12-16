namespace Sanasoppa.Core.Exceptions;

public class AlreadyInGameException : ApplicationException
{
    public AlreadyInGameException() : base()
    {
    }

    public AlreadyInGameException(string message) : base(message)
    {
    }

    public AlreadyInGameException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
