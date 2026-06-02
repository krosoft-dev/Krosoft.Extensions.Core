using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Models;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class KrosoftFileExtensionsTests
{
    [TestMethod]
    public async Task ToMultipartFormDataContent_Ok()
    {
        var contentBytes = "Hello World"u8.ToArray();
        var file = new KrosoftFile("document.pdf", contentBytes);

        var result = file.ToMultipartFormDataContent();

        Check.That(result).IsNotNull();

        var parts = result.ToList();
        Check.That(parts).HasSize(1);

        var part = parts[0];
        var disposition = part.Headers.ContentDisposition!;
        Check.That(disposition.Name).IsEqualTo("file");
        Check.That(disposition.FileName).IsEqualTo("document.pdf");

        var bytes = await part.ReadAsByteArrayAsync();
        Check.That(bytes).IsEqualTo(contentBytes);
    }

    [TestMethod]
    public void ToMultipartFormDataContent_WithEmptyContent_Ok()
    {
        var file = new KrosoftFile("empty.txt", Array.Empty<byte>());

        var result = file.ToMultipartFormDataContent();

        Check.That(result).IsNotNull();
        var parts = result.ToList();
        Check.That(parts).HasSize(1);

        var disposition = parts[0].Headers.ContentDisposition!;
        Check.That(disposition.Name).IsEqualTo("file");
        Check.That(disposition.FileName).IsEqualTo("empty.txt");
    }

    [TestMethod]
    public async Task ToMultipartFormDataContent_PreservesFileName()
    {
        var file = new KrosoftFile("rapport annuel 2024.xlsx", new byte[] { 1, 2, 3 });

        var result = file.ToMultipartFormDataContent();

        var part = result.ToList()[0];
        Check.That(part.Headers.ContentDisposition!.FileName).IsEqualTo("rapport annuel 2024.xlsx");

        var bytes = await part.ReadAsByteArrayAsync();
        Check.That(bytes).IsEqualTo(new byte[] { 1, 2, 3 });
    }
}
