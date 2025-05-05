using Krosoft.Extensions.Core.Models;

namespace Krosoft.Extensions.Core.Extensions;

public static class ExceptionExtensions
{
    public static Result<T> ToFailureResult<T>(this Exception ex) => Result<T>.Failure(ex);
}