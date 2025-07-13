namespace Backend.Application.Errors;

public class PurchaseException : Exception
{
    public PurchaseErrorEnum ErrorType { get; }

    public PurchaseException(PurchaseErrorEnum errorType, string message)
        : base(message)
    {
        ErrorType = errorType;
    }
}