namespace Krosoft.Extensions.Core.Extensions;

public static class ByteExtensions
{
    public static IEnumerable<Stream> ToStreams(this IEnumerable<byte[]> files)
    {
        var streams = new List<Stream>();

        foreach (var file in files)
        {
            streams.Add(new MemoryStream(file));
        }

        return streams;
    }
}