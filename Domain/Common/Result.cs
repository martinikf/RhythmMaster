namespace Domain.Common;

public abstract class Result
{
    public bool Success { get; protected set; }
    public bool Failure => !Success;
}

public abstract class Result<T> : Result
{
    public T Data;

    protected Result(T data)
    {
        Data = data;
    }
    
}

public class SuccessResult : Result
{
    public SuccessResult()
    {
        Success = true;
    }
}

public class SuccessResult<T> : Result<T>
{
    public SuccessResult(T data) : base(data)
    {
        Success = true;
    }
}

public class ErrorResult : Result
{
    public string Message { get; }
    
    public ErrorResult(string message)
    {
        Success = false;
        Message = message;

    }
} 

public class ErrorResult<T> : Result<T>
{
    public string Message { get; }
    
    public ErrorResult(string message) : base(default!)
    {
        Success = false;
        Message = message;

    }
}

public class NotFoundError : ErrorResult
{
    public NotFoundError(string message) : base(message) { }
}

public class NotFoundError<T> : ErrorResult<T>
{
    public NotFoundError(string message) : base(message) { }
}

public class ValidationError : ErrorResult
{
    public ValidationError(string message) : base(message){}
}

public class ValidationError<T> : ErrorResult<T>
{
    public ValidationError(string message) : base(message){}
}