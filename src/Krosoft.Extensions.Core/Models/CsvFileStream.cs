namespace Krosoft.Extensions.Core.Models;

public record CsvFileStream : GenericFileStream
{
    public CsvFileStream(Stream stream,
                         string fileName)
        : base(stream, fileName, "text/csv")
    {
    }
}