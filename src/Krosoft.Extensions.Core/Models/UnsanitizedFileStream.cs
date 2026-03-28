namespace Krosoft.Extensions.Core.Models;

public record UnsanitizedFileStream(Stream Stream, string FileName, string ContentType) : IFileStream;