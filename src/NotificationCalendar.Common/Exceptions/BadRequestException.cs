namespace NotificationCalendar.Common.Exceptions;

public class BadRequestException : Exception
{
    private Dictionary<string, string[]> _errors = new Dictionary<string, string[]>();

    public BadRequestException(string errorMessage) : base(errorMessage)
    {
        Message = errorMessage;
    }

    public BadRequestException(string propertyName, string errorMessage) : base(errorMessage)
    {
        Message = errorMessage;
        _errors.Add(propertyName, [errorMessage]);
    }

    public BadRequestException(Dictionary<string, string[]> errors) : base()
    {
        _errors = errors;
    }

    public virtual string Message { get; }

    public Dictionary<string, string[]> Errors => _errors;
}
