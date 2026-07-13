using System.Text;
using Krosoft.Extensions.Core.Services;

namespace Krosoft.Extensions.Core.Tests.Services;

[TestClass]
public class StringWriterWithEncodingTests
{
    [TestMethod]
    public void Encoding_WhenUtf8_ReturnsUtf8()
    {
        using var writer = new StringWriterWithEncoding(Encoding.UTF8);

        Check.That(writer.Encoding).IsEqualTo(Encoding.UTF8);
    }

    [TestMethod]
    public void Encoding_WhenUnicode_ReturnsUnicode()
    {
        using var writer = new StringWriterWithEncoding(Encoding.Unicode);

        Check.That(writer.Encoding).IsEqualTo(Encoding.Unicode);
    }

    [TestMethod]
    public void Utf8_ReturnsWriterWithUtf8Encoding()
    {
        using var writer = StringWriterWithEncoding.Utf8();

        Check.That(writer).IsInstanceOf<StringWriterWithEncoding>();
        Check.That(writer.Encoding).IsEqualTo(Encoding.UTF8);
    }

    [TestMethod]
    public void Write_ThenToString_ReturnsWrittenContent()
    {
        using var writer = StringWriterWithEncoding.Utf8();

        writer.Write("héllo wörld");

        Check.That(writer.ToString()).IsEqualTo("héllo wörld");
    }
}
