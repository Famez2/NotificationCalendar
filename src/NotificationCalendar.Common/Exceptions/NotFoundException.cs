namespace NotificationCalendar.Common.Exceptions;

public class NotFoundException : Exception
{
    private Dictionary<string, string[]> _errors = new Dictionary<string, string[]>();

    public NotFoundException(string errorMessage) : base(errorMessage)
    {
        Message = errorMessage;
    }

    public NotFoundException(string propertyName, string errorMessage) : base(errorMessage)
    {
        Message = errorMessage;
        _errors.Add(propertyName, [errorMessage]);
    }

    public NotFoundException(Dictionary<string, string[]> errors) : base()
    {
        _errors = errors;
    }

    public virtual string Message { get; }

    public Dictionary<string, string[]> Errors => _errors;
}
