using System.Reflection;
using System.Text;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class FileTypeHelperTests
{
    [TestMethod]
    public void IsZip_WithValidZipBytes_ReturnsTrue()
    {
        var fileBytes = new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x00, 0x00 };

        var result = FileTypeHelper.IsZip(fileBytes);

        Check.That(result).IsTrue();
    }

    [TestMethod]
    public void IsZip_WithInvalidZipBytes_ReturnsFalse()
    {
        var fileBytes = new byte[] { 0x00, 0x00, 0x00, 0x00 };

        var result = FileTypeHelper.IsZip(fileBytes);

        Check.That(result).IsFalse();
    }

    [TestMethod]
    public void IsZip_WithTooShortBytes_ReturnsFalse()
    {
        var fileBytes = new byte[] { 0x50, 0x4B, 0x03 };

        var result = FileTypeHelper.IsZip(fileBytes);

        Check.That(result).IsFalse();
    }

    [TestMethod]
    public void IsZip_WithNullBytes_ThrowsException()
    {
        Check.ThatCode(() => FileTypeHelper.IsZip(null!))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'fileBytes' n'est pas renseignée.");
    }

    [TestMethod]
    public void IsZip_Ok()
    {
        var fileBytesXml = Assembly.GetExecutingAssembly().Read("xml.xml").ReadAsByteArray();
        Check.That(FileTypeHelper.IsZip(fileBytesXml)).IsFalse();

        var fileBytesPdf = Assembly.GetExecutingAssembly().Read("sample1.pdf").ReadAsByteArray();
        Check.That(FileTypeHelper.IsZip(fileBytesPdf)).IsFalse();

        var fileBytesZip = Assembly.GetExecutingAssembly().Read("import_assets.zip").ReadAsByteArray();
        Check.That(FileTypeHelper.IsZip(fileBytesZip)).IsTrue();

        var fileBytesEdi = Assembly.GetExecutingAssembly().Read("edi.edi").ReadAsByteArray();
        Check.That(FileTypeHelper.IsZip(fileBytesEdi)).IsFalse();
    }

    [TestMethod]
    public void IsPdf_WithValidPdfBytes_ReturnsTrue()
    {
        var fileBytes = Encoding.UTF8.GetBytes("%PDF-1.5");

        var result = FileTypeHelper.IsPdf(fileBytes);

        Check.That(result).IsTrue();
    }

    [TestMethod]
    public void IsPdf_WithInvalidPdfBytes_ReturnsFalse()
    {
        var fileBytes = Encoding.UTF8.GetBytes("NOTAPDF");

        var result = FileTypeHelper.IsPdf(fileBytes);

        Check.That(result).IsFalse();
    }

    [TestMethod]
    public void IsPdf_WithNullBytes_ThrowsException()
    {
        Check.ThatCode(() => FileTypeHelper.IsPdf(null!))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'fileBytes' n'est pas renseignée.");
    }

    [TestMethod]
    public void IsPdf_Ok()
    {
        var fileBytesXml = Assembly.GetExecutingAssembly().Read("xml.xml").ReadAsByteArray();
        Check.That(FileTypeHelper.IsPdf(fileBytesXml)).IsFalse();

        var fileBytesPdf = Assembly.GetExecutingAssembly().Read("sample1.pdf").ReadAsByteArray();
        Check.That(FileTypeHelper.IsPdf(fileBytesPdf)).IsTrue();

        var fileBytesZip = Assembly.GetExecutingAssembly().Read("import_assets.zip").ReadAsByteArray();
        Check.That(FileTypeHelper.IsPdf(fileBytesZip)).IsFalse();

        var fileBytesEdi = Assembly.GetExecutingAssembly().Read("edi.edi").ReadAsByteArray();
        Check.That(FileTypeHelper.IsPdf(fileBytesEdi)).IsFalse();
    }

    [TestMethod]
    public void IsXml_WithValidXmlBytes_ReturnsTrue()
    {
        var fileBytes = Encoding.UTF8.GetBytes("<?xml version=\"1.0\"?>");

        var result = FileTypeHelper.IsXml(fileBytes);

        Check.That(result).IsTrue();
    }

    [TestMethod]
    public void IsXml_WithInvalidXmlBytes_ReturnsFalse()
    {
        var fileBytes = Encoding.UTF8.GetBytes("<root>");

        var result = FileTypeHelper.IsXml(fileBytes);

        Check.That(result).IsFalse();
    }

    [TestMethod]
    public void IsXml_WithNullBytes_ThrowsException()
    {
        Check.ThatCode(() => FileTypeHelper.IsXml(null!))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'fileBytes' n'est pas renseignée.");
    }

    [TestMethod]
    public void IsXml_Ok()
    {
        var fileBytesXml = Assembly.GetExecutingAssembly().Read("xml.xml").ReadAsByteArray();
        Check.That(FileTypeHelper.IsXml(fileBytesXml)).IsTrue();

        var fileBytesPdf = Assembly.GetExecutingAssembly().Read("sample1.pdf").ReadAsByteArray();
        Check.That(FileTypeHelper.IsXml(fileBytesPdf)).IsFalse();

        var fileBytesZip = Assembly.GetExecutingAssembly().Read("import_assets.zip").ReadAsByteArray();
        Check.That(FileTypeHelper.IsXml(fileBytesZip)).IsFalse();

        var fileBytesEdi = Assembly.GetExecutingAssembly().Read("edi.edi").ReadAsByteArray();
        Check.That(FileTypeHelper.IsXml(fileBytesEdi)).IsFalse();
    }

    [TestMethod]
    public void IsJson_WithValidJsonObjectBytes_ReturnsTrue()
    {
        var fileBytes = Encoding.UTF8.GetBytes("{\"key\":\"value\"}");

        var result = FileTypeHelper.IsJson(fileBytes);

        Check.That(result).IsTrue();
    }

    [TestMethod]
    public void IsJson_WithValidJsonArrayBytes_ReturnsTrue()
    {
        var fileBytes = Encoding.UTF8.GetBytes("[1,2,3]");

        var result = FileTypeHelper.IsJson(fileBytes);

        Check.That(result).IsTrue();
    }

    [TestMethod]
    public void IsJson_WithInvalidJsonBytes_ReturnsFalse()
    {
        var fileBytes = Encoding.UTF8.GetBytes("Not JSON");

        var result = FileTypeHelper.IsJson(fileBytes);

        Check.That(result).IsFalse();
    }

    [TestMethod]
    public void IsJson_WithNullBytes_ThrowsException()
    {
        Check.ThatCode(() => FileTypeHelper.IsJson(null!))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'fileBytes' n'est pas renseignée.");
    }

    [TestMethod]
    public void IsJson_Ok()
    {
        var fileBytesXml = Assembly.GetExecutingAssembly().Read("xml.xml").ReadAsByteArray();
        Check.That(FileTypeHelper.IsJson(fileBytesXml)).IsFalse();

        var fileBytesPdf = Assembly.GetExecutingAssembly().Read("sample1.pdf").ReadAsByteArray();
        Check.That(FileTypeHelper.IsJson(fileBytesPdf)).IsFalse();

        var fileBytesZip = Assembly.GetExecutingAssembly().Read("import_assets.zip").ReadAsByteArray();
        Check.That(FileTypeHelper.IsJson(fileBytesZip)).IsFalse();

        var fileBytesEdi = Assembly.GetExecutingAssembly().Read("edi.edi").ReadAsByteArray();
        Check.That(FileTypeHelper.IsJson(fileBytesEdi)).IsFalse();
    }

    [TestMethod]
    public void IsGZip_WithValidGZipBytes_ReturnsTrue()
    {
        var fileBytes = new byte[] { 0x1F, 0x8B, 0x08, 0x00, 0x00 };

        var result = FileTypeHelper.IsGZip(fileBytes);

        Check.That(result).IsTrue();
    }

    [TestMethod]
    public void IsGZip_WithInvalidGZipBytes_ReturnsFalse()
    {
        var fileBytes = new byte[] { 0x00, 0x00, 0x00 };

        var result = FileTypeHelper.IsGZip(fileBytes);

        Check.That(result).IsFalse();
    }

    [TestMethod]
    public void IsGZip_WithTooShortBytes_ReturnsFalse()
    {
        var fileBytes = new byte[] { 0x1F, 0x8B };

        var result = FileTypeHelper.IsGZip(fileBytes);

        Check.That(result).IsFalse();
    }

    [TestMethod]
    public void IsGZip_WithNullBytes_ThrowsException()
    {
        Check.ThatCode(() => FileTypeHelper.IsGZip(null!))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'fileBytes' n'est pas renseignée.");
    }

    [TestMethod]
    public void IsGZip_Ok()
    {
        var fileBytesXml = Assembly.GetExecutingAssembly().Read("xml.xml").ReadAsByteArray();
        Check.That(FileTypeHelper.IsGZip(fileBytesXml)).IsFalse();

        var fileBytesPdf = Assembly.GetExecutingAssembly().Read("sample1.pdf").ReadAsByteArray();
        Check.That(FileTypeHelper.IsGZip(fileBytesPdf)).IsFalse();

        var fileBytesZip = Assembly.GetExecutingAssembly().Read("import_assets.zip").ReadAsByteArray();
        Check.That(FileTypeHelper.IsGZip(fileBytesZip)).IsFalse();

        var fileBytesEdi = Assembly.GetExecutingAssembly().Read("edi.edi").ReadAsByteArray();
        Check.That(FileTypeHelper.IsGZip(fileBytesEdi)).IsFalse();
    }

    [TestMethod]
    public void IsEdifact_WithValidUNAFormat_ReturnsTrue()
    {
        // UNA suivi de 6 caractères de service (séparateurs)
        var fileBytes = Encoding.ASCII.GetBytes("UNA:+.? 'UNB+UNOA:2+SENDER+RECEIVER+220101:1200+12345678'");

        var result = FileTypeHelper.IsEdifact(fileBytes);

        Check.That(result).IsTrue();
    }

    [TestMethod]
    public void IsEdifact_WithValidUNBFormat_ReturnsTrue()
    {
        // Format EDIFACT commençant directement par UNB
        var fileBytes = Encoding.ASCII.GetBytes("UNB+UNOA:2+SENDER+RECEIVER+220101:1200+12345678'UNH+1+INVOIC:D:96A:UN:EAN008'");

        var result = FileTypeHelper.IsEdifact(fileBytes);

        Check.That(result).IsTrue();
    }

    [TestMethod]
    public void IsEdifact_WithInvalidFormat_ReturnsFalse()
    {
        var fileBytes = Encoding.ASCII.GetBytes("This is not an EDIFACT file");

        var result = FileTypeHelper.IsEdifact(fileBytes);

        Check.That(result).IsFalse();
    }

    [TestMethod]
    public void IsEdifact_WithIncompleteUNAFormat_ReturnsFalse()
    {
        // UNA avec moins de 6 caractères qui suivent
        var fileBytes = Encoding.ASCII.GetBytes("UNA:+.");

        var result = FileTypeHelper.IsEdifact(fileBytes);

        Check.That(result).IsFalse();
    }

    [TestMethod]
    public void IsEdifact_WithUNBWithoutUNH_ReturnsFalse()
    {
        // UNB sans UNH
        var fileBytes = Encoding.ASCII.GetBytes("UNB+UNOA:2+SENDER+RECEIVER+220101:1200+12345678'");

        var result = FileTypeHelper.IsEdifact(fileBytes);

        Check.That(result).IsFalse();
    }

    [TestMethod]
    public void IsEdifact_WithTooShortBytes_ReturnsFalse()
    {
        var fileBytes = new byte[] { 0x55, 0x4E, 0x41 }; // "UNA" en ASCII

        var result = FileTypeHelper.IsEdifact(fileBytes);

        Check.That(result).IsFalse();
    }

    [TestMethod]
    public void IsEdifact_WithNullBytes_ThrowsException()
    {
        Check.ThatCode(() => FileTypeHelper.IsEdifact(null!))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'fileBytes' n'est pas renseignée.");
    }

    [TestMethod]
    public void IsEdifact_Ok()
    {
        var fileBytesXml = Assembly.GetExecutingAssembly().Read("xml.xml").ReadAsByteArray();
        Check.That(FileTypeHelper.IsEdifact(fileBytesXml)).IsFalse();

        var fileBytesPdf = Assembly.GetExecutingAssembly().Read("sample1.pdf").ReadAsByteArray();
        Check.That(FileTypeHelper.IsEdifact(fileBytesPdf)).IsFalse();

        var fileBytesZip = Assembly.GetExecutingAssembly().Read("import_assets.zip").ReadAsByteArray();
        Check.That(FileTypeHelper.IsEdifact(fileBytesZip)).IsFalse();

        var fileBytesEdi = Assembly.GetExecutingAssembly().Read("edi.edi").ReadAsByteArray();
        Check.That(FileTypeHelper.IsEdifact(fileBytesEdi)).IsTrue();
    }
}