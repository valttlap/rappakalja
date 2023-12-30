namespace Sanasoppa.Core.Exceptions;

public class NotEnoughPlayersException : ApplicationException
{
    public NotEnoughPlayersException() : base()
    {
    }

    public NotEnoughPlayersException(string message) : base(message)
    {
    }

    public NotEnoughPlayersException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
