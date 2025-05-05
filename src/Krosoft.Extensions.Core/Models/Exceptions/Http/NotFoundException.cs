using System.Net;

namespace Krosoft.Extensions.Core.Models.Exceptions.Http;

public class NotFoundException : HttpException
{
    public NotFoundException(string why)
        : base(HttpStatusCode.NotFound, why)
    {
    }
}