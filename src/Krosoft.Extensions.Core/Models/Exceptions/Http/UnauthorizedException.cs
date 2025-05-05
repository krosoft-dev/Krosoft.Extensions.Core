using System.Net;

namespace Krosoft.Extensions.Core.Models.Exceptions.Http;

public class UnauthorizedException : HttpException
{
    public UnauthorizedException(string why)
        : base(HttpStatusCode.Unauthorized, why)
    {
    }
}