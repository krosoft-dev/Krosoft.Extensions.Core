using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Core.Models;

public readonly struct Result<T>
{
    public T? Value { get; }
    public Exception? Exception { get; }

    public Result(T value)
    {
        Value = value;
        Exception = null;
    }

    public Result(Exception e)
    {
        Exception = e;
        Value = default;
    }

    public T Validate()
    {
        if (IsFaulted)
        {
            throw new KrosoftFunctionalException(Exception!.Message);
        }

        return Value!;
    }

    public bool IsSuccess => Exception == null;
    public bool IsFaulted => !IsSuccess;

    public static Result<T> Success(T value) => new Result<T>(value);

    public static Result<T> Failure(Exception e) => new Result<T>(e);

    public Result<TU> Map<TU>(Func<T, TU> transform) =>
        IsSuccess
            ? Result<TU>.Success(transform(Value!))
            : Result<TU>.Failure(Exception!);

    public Result<TU> Bind<TU>(Func<T, Result<TU>> transform) =>
        IsSuccess
            ? transform(Value!)
            : Result<TU>.Failure(Exception!);
}