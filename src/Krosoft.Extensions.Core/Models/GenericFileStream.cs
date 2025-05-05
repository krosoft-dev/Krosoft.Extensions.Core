using Krosoft.Extensions.Core.Extensions;

namespace Krosoft.Extensions.Core.Models;

public record GenericFileStream : IFileStream
{
    public GenericFileStream(Stream stream, string fileName, string contentType)
    {
        Stream = stream;
        FileName = fileName.Sanitize() ?? fileName;
        ContentType = contentType;
    }

    public string ContentType { get; }
    public string FileName { get; }
    public Stream Stream { get; }
}