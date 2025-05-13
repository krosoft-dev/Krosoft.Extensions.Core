using System.Text;

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

    public static string ToHex(this byte[] bytes, bool upperCase)
    {
        var result = new StringBuilder(bytes.Length * 2);

        for (var i = 0; i < bytes.Length; i++)
        {
            result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
        }

        return result.ToString();
    }
}