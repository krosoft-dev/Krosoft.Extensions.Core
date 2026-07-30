using System.Net.Mime;
using Krosoft.Extensions.Core.Models;

namespace Krosoft.Extensions.Core.Tests.Models;

[TestClass]
public class XmlFileStreamTests
{
    [TestMethod]
    public void Constructor_SetsCorrectContentType()
    {
        using var stream = new MemoryStream();
        var xml = new XmlFileStream(stream, "facture.xml");

        Check.That(xml.ContentType).IsEqualTo(MediaTypeNames.Application.Xml);
    }

    [TestMethod]
    public void Constructor_SetsFileName()
    {
        using var stream = new MemoryStream();
        var xml = new XmlFileStream(stream, "facture.xml");

        Check.That(xml.FileName).IsEqualTo("facture.xml");
    }

    [TestMethod]
    public void Constructor_SetsStream()
    {
        using var stream = new MemoryStream();
        var xml = new XmlFileStream(stream, "facture.xml");

        Check.That(xml.Stream).IsSameReferenceAs(stream);
    }

    [TestMethod]
    public void Constructor_SanitizesFileName()
    {
        using var stream = new MemoryStream();
        var xml = new XmlFileStream(stream, "facture:2024.xml");

        Check.That(xml.FileName).IsEqualTo("facture_2024.xml");
    }
}
