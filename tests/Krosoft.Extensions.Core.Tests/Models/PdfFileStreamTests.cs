using System.Net.Mime;
using Krosoft.Extensions.Core.Models;

namespace Krosoft.Extensions.Core.Tests.Models;

[TestClass]
public class PdfFileStreamTests
{
    [TestMethod]
    public void Constructor_SetsCorrectContentType()
    {
        using var stream = new MemoryStream();
        var pdf = new PdfFileStream(stream, "rapport.pdf");

        Check.That(pdf.ContentType).IsEqualTo(MediaTypeNames.Application.Pdf);
    }

    [TestMethod]
    public void Constructor_SetsFileName()
    {
        using var stream = new MemoryStream();
        var pdf = new PdfFileStream(stream, "rapport.pdf");

        Check.That(pdf.FileName).IsEqualTo("rapport.pdf");
    }

    [TestMethod]
    public void Constructor_SetsStream()
    {
        using var stream = new MemoryStream();
        var pdf = new PdfFileStream(stream, "rapport.pdf");

        Check.That(pdf.Stream).IsSameReferenceAs(stream);
    }

    [TestMethod]
    public void Constructor_SanitizesFileName()
    {
        using var stream = new MemoryStream();
        var pdf = new PdfFileStream(stream, "rapport:2024.pdf");

        Check.That(pdf.FileName).IsEqualTo("rapport_2024.pdf");
    }
}
