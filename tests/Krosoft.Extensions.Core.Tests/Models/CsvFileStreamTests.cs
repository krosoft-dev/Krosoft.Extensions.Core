using Krosoft.Extensions.Core.Models;

namespace Krosoft.Extensions.Core.Tests.Models;

[TestClass]
public class CsvFileStreamTests
{
    [TestMethod]
    public void Constructor_SetsCorrectContentType()
    {
        using var stream = new MemoryStream();
        var csv = new CsvFileStream(stream, "export.csv");

        Check.That(csv.ContentType).IsEqualTo("text/csv");
    }

    [TestMethod]
    public void Constructor_SetsFileName()
    {
        using var stream = new MemoryStream();
        var csv = new CsvFileStream(stream, "export.csv");

        Check.That(csv.FileName).IsEqualTo("export.csv");
    }

    [TestMethod]
    public void Constructor_SetsStream()
    {
        using var stream = new MemoryStream();
        var csv = new CsvFileStream(stream, "export.csv");

        Check.That(csv.Stream).IsSameReferenceAs(stream);
    }

    [TestMethod]
    public void Constructor_SanitizesFileName()
    {
        using var stream = new MemoryStream();
        var csv = new CsvFileStream(stream, "export<invalid>.csv");

        Check.That(csv.FileName).IsEqualTo("export_invalid_.csv");
    }
}
