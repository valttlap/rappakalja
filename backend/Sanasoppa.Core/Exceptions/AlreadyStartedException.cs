namespace Sanasoppa.Core.Exceptions;

public class AlreadyStartedException : ApplicationException
{
    public AlreadyStartedException() : base()
    {
    }

    public AlreadyStartedException(string message) : base(message)
    {
    }

    public AlreadyStartedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
