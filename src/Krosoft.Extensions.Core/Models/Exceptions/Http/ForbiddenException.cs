using System.Net;

namespace Krosoft.Extensions.Core.Models.Exceptions.Http;

public class ForbiddenException : HttpException
{
    public ForbiddenException(string why)
        : base(HttpStatusCode.Forbidden, why)
    {
    }
}