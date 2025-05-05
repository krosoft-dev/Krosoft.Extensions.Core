using System.Net;

namespace Krosoft.Extensions.Core.Models.Exceptions.Http;

public class HttpException : KrosoftException
{
    public HttpException(HttpStatusCode code,
                         string? message) : this(code, message, null)
    {
        Code = code;
    }

    public HttpException(HttpStatusCode code,
                         string? message,
                         Exception? innerException) : base(message ?? string.Empty, innerException)
    {
        Code = code;
    }

    public HttpStatusCode Code { get; }
}