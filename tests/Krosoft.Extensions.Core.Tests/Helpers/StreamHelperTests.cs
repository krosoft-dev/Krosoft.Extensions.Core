using Krosoft.Extensions.Core.Helpers;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class StreamHelperTests
{
    [TestMethod]
    public void GenerateStreamFromString_Ok()
    {
        using var stream = StreamHelper.GenerateStreamFromString("test");
        Check.That(stream).IsNotNull();
        Check.That(stream.CanRead).IsTrue();

        using var reader = new StreamReader(stream);
        var text = reader.ReadToEnd();
        Check.That(text).IsNotNull();
        Check.That(text).IsEqualTo("test");
    }
}