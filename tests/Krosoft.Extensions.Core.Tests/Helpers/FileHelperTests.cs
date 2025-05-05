using Krosoft.Extensions.Core.Helpers;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class FileHelperTests
{
    [TestMethod]
    public async Task WriteTextAsync_Ok()
    {
        await FileHelper.WriteTextAsync("hello.txt", "Hello World", CancellationToken.None);
        var file = await FileHelper.ReadAsStringAsync("hello.txt", CancellationToken.None);

        Check.That(file).IsNotNull();
        Check.That(file.Length).IsEqualTo(11);
        Check.That(file).IsEqualTo("Hello World");
    }

    [TestMethod]
    public async Task ReadAsBytesAsync_Ok()
    {
        await FileHelper.WriteTextAsync("hello.txt", "Hello World", CancellationToken.None);

        var bytes = await FileHelper.ReadAsBytesAsync("hello.txt", CancellationToken.None);

        Check.That(bytes).IsNotNull();
        Check.That(bytes.Length).IsEqualTo(11);
        Check.That(bytes).IsEqualTo(new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100 });
    }

    [TestMethod]
    public async Task ReadAsBytes_Ok()
    {
        await FileHelper.WriteTextAsync("hello.txt", "Hello World", CancellationToken.None);

        var bytes = FileHelper.ReadAsBytes("hello.txt");

        Check.That(bytes).IsNotNull();
        Check.That(bytes.Length).IsEqualTo(11);
        Check.That(bytes).IsEqualTo(new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100 });
    }
}