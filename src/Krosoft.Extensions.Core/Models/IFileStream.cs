namespace Krosoft.Extensions.Core.Models;

public interface IFileStream
{
    Stream Stream { get; }
    string FileName { get; }
    string ContentType { get; }
}