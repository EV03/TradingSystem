public abstract class OperationResultBase
{
    public bool IsSuccess { get; }
    public bool IsCanceled { get; }
    public string? ErrorMessage { get; }

    protected OperationResultBase(bool isSuccess, bool isCanceled, string? errorMessage)
    {
        IsSuccess = isSuccess;
        IsCanceled = isCanceled;
        ErrorMessage = errorMessage;
    }
}

public class OperationResult : OperationResultBase
{
    private OperationResult(bool isSuccess, bool isCanceled, string? errorMessage)
        : base(isSuccess, isCanceled, errorMessage) { }

    public static OperationResult Success() => new OperationResult(true, false, null);
    public static OperationResult Canceled() => new OperationResult(false, true, null);
    public static OperationResult Failure(string errorMessage) => new OperationResult(false, false, errorMessage);
}

public class OperationResult<T> : OperationResultBase
{
    public T? Result { get; }

    private OperationResult(bool isSuccess, bool isCanceled, string? errorMessage, T? result)
        : base(isSuccess, isCanceled, errorMessage)
    {
        Result = result;
    }

    public static OperationResult<T> Success(T result) => new OperationResult<T>(true, false, null, result);
    public static OperationResult<T> Canceled() => new OperationResult<T>(false, true, null, default);
    public static OperationResult<T> Failure(string errorMessage) => new OperationResult<T>(false, false, errorMessage, default);
}