using System.Net.Mime;

namespace Krosoft.Extensions.Core.Models;

public record XmlFileStream : GenericFileStream
{
    public XmlFileStream(Stream stream,
                         string fileName)
        : base(stream, fileName, MediaTypeNames.Application.Xml)
    {
    }
}
