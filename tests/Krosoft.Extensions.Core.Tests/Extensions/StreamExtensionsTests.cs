using System.Text;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Helpers;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class StreamExtensionsTests
{
    [TestMethod]
    public void ToBase64_MemoryStream_Ok()
    {
        using var stream = StreamHelper.GenerateStreamFromString("test");
        var base64 = stream.ToBase64();
        Check.That(base64).IsEqualTo("dGVzdA==");
    }

    [TestMethod]
    public void ToBase64_Ok()
    {
        using (var fs = File.Create("test.txt"))
        {
            var info = new UTF8Encoding(true).GetBytes("test");
            fs.Write(info, 0, info.Length);

            var base64 = fs.ToBase64();
            Check.That(base64).IsEqualTo("dGVzdA==");
        }
    }

    [TestMethod]
    public void ToByte_Ok()
    {
        using var stream = StreamHelper.GenerateStreamFromString("test");
        var bytes = stream.ToByte();
        Check.That(ByteHelper.GetString(bytes)).IsEqualTo("test");
    }
}