using Krosoft.Extensions.Core.Extensions;

namespace Krosoft.Extensions.Core.Models;

public record GenericFileStream : UnsanitizedFileStream
{
    public GenericFileStream(Stream stream, string fileName, string contentType)
        : base(stream, fileName.Sanitize() ?? fileName, contentType)
    {
    }
}