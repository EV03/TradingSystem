namespace Domain.CashDesk;

public class OperationResult<T>
{
    public bool IsSuccess { get; }
    public bool IsCanceled { get; }
    public string? ErrorMessage { get; }
    public T? Result { get; }

    private OperationResult(bool isSuccess, bool isCanceled, string? errorMessage, T? result)
    {
        IsSuccess = isSuccess;
        IsCanceled = isCanceled;
        ErrorMessage = errorMessage;
        Result = result;
    }

    public static OperationResult<T> Success(T result) => new OperationResult<T>(true, false, null, result);
    public static OperationResult<T> Canceled() => new OperationResult<T>(false, true, null, default);
    public static OperationResult<T> Failure(string errorMessage) => new OperationResult<T>(false, false, errorMessage, default);
}
