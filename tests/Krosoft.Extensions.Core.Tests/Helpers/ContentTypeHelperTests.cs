using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class ContentTypeHelperTests
{
    [TestMethod]
    public void ContentTypeHelper_Pdf()
    {
        Check.That(ContentTypeHelper.GetContentType(".pdf")).IsEqualTo("application/pdf");
    }

    [TestMethod]
    public void ContentTypeHelper_Json()
    {
        Check.That(ContentTypeHelper.GetContentType(".json")).IsEqualTo("application/json");
    }

    [TestMethod]
    public void ContentTypeHelper_Zip()
    {
        Check.That(ContentTypeHelper.GetContentType(".zip")).IsEqualTo("application/zip");
    }

    [TestMethod]
    public void ContentTypeHelper_Html()
    {
        Check.That(ContentTypeHelper.GetContentType(".html")).IsEqualTo("text/html");
    }

    [TestMethod]
    public void ContentTypeHelper_TarGz()
    {
        Check.That(ContentTypeHelper.GetContentType(".tar.gz")).IsEqualTo("application/octet-stream");
    }

    [TestMethod]
    public void ContentTypeHelper_Empty()
    {
        Check.That(ContentTypeHelper.GetContentType(string.Empty)).IsEqualTo("application/octet-stream");
    }

    [TestMethod]
    public void GetContentType_WithTxtExtension_ReturnsTextPlain()
    {
        var result = ContentTypeHelper.GetContentType(".txt");

        Check.That(result).IsEqualTo("text/plain");
    }

    [TestMethod]
    public void GetContentType_WithPdfExtension_ReturnsApplicationPdf()
    {
        var result = ContentTypeHelper.GetContentType(".pdf");

        Check.That(result).IsEqualTo("application/pdf");
    }

    [TestMethod]
    public void GetContentType_WithUnknownExtension_ReturnsOctetStream()
    {
        var result = ContentTypeHelper.GetContentType(".unknown");

        Check.That(result).IsEqualTo("application/octet-stream");
    }

    [TestMethod]
    public void GetContentType_WithNullExtension_Exception()
    {
        Check.ThatCode(() => ContentTypeHelper.GetContentType(null!))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'fileExtension' n'est pas renseignée.");
    }

    [TestMethod]
    public void GetContentType_WithEmptyExtension_ReturnsOctetStream()
    {
        var result = ContentTypeHelper.GetContentType(string.Empty);

        Check.That(result).IsEqualTo("application/octet-stream");
    }

    [TestMethod]
    public void GetContentType_WithDifferentCase_ReturnsCorrectContentType()
    {
        var result = ContentTypeHelper.GetContentType(".PDF");

        Check.That(result).IsEqualTo("application/pdf");
    }

    [DataTestMethod]
    [DataRow(".txt", "text/plain")]
    [DataRow(".pdf", "application/pdf")]
    [DataRow(".doc", "application/msword")]
    [DataRow(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
    [DataRow(".xls", "application/vnd.ms-excel")]
    [DataRow(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    [DataRow(".png", "image/png")]
    [DataRow(".jpg", "image/jpeg")]
    [DataRow(".jpeg", "image/jpeg")]
    [DataRow(".gif", "image/gif")]
    [DataRow(".csv", "text/csv")]
    [DataRow(".html", "text/html")]
    [DataRow(".htm", "text/html")]
    [DataRow(".css", "text/css")]
    [DataRow(".js", "application/javascript")]
    [DataRow(".json", "application/json")]
    [DataRow(".mp3", "audio/mpeg")]
    [DataRow(".mp4", "video/mp4")]
    [DataRow(".zip", "application/zip")]
    public void GetContentType_Ok(string extension, string expectedContentType)
    {
        var result = ContentTypeHelper.GetContentType(extension);

        Check.That(result).IsEqualTo(expectedContentType);
    }
}