using System.Net.Mime;

namespace Krosoft.Extensions.Core.Models;

public record ZipFileStream : GenericFileStream
{
    public ZipFileStream(Stream stream,
                         string fileName)
        : base(stream, fileName, MediaTypeNames.Application.Zip)
    {
    }
}