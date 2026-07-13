using System.Text;

namespace Krosoft.Extensions.Core.Services;

public class StringWriterWithEncoding : StringWriter
{
    private readonly Encoding _encoding;

    public StringWriterWithEncoding(Encoding encoding)
    {
        _encoding = encoding;
    }

    public override Encoding Encoding => _encoding;

    public static StringWriterWithEncoding Utf8() => new(Encoding.UTF8);
}
