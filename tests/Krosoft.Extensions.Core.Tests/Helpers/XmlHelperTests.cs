using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Samples.Library.Models.Xml;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class XmlHelperTests
{
    [TestMethod]
    public void Deserialize_Stream_Null()
    {
        var obj = XmlHelper.Deserialize<DepotXml>((Stream?)null);
        Check.That(obj).IsNull();
    }

    [TestMethod]
    public void Deserialize_String_Null()
    {
        var obj = XmlHelper.Deserialize<DepotXml>((string?)null);
        Check.That(obj).IsNull();
    }

    [TestMethod]
    public void Deserialize_Ok_Content()
    {
        var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><header xmlns=\"mfp:anaf:dgti:spv:respUploadFisier:v1\" dateResponse=\"202503312230\" ExecutionStatus=\"0\" index_incarcare=\"5020282769\"/>";
        var obj = XmlHelper.Deserialize<DepotXml>(xml);
        Check.That(obj).IsNotNull();
        Check.That(obj?.DateResponse).IsEqualTo("202503312230");
        Check.That(obj?.ExecutionStatus).IsEqualTo("0");
        Check.That(obj?.NumeroFluxDepot).IsEqualTo("5020282769");
        Check.That(obj?.Errors).IsEmpty();
    }

    [TestMethod]
    public void Deserialize_Ok_Error()
    {
        var xml =
            "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><header xmlns=\"mfp:anaf:dgti:spv:respUploadFisier:v1\" dateResponse=\"202503312232\" ExecutionStatus=\"1\"><Errors errorMessage=\"Fisierul transmis nu este valid. org.xml.sax.SAXParseException; lineNumber: 1; columnNumber: 540; cvc-elt.1.a: Cannot find the declaration of element 'Invoice'.\"/></header>";
        var obj = XmlHelper.Deserialize<DepotXml>(xml);
        Check.That(obj).IsNotNull();
        Check.That(obj?.DateResponse).IsEqualTo("202503312232");
        Check.That(obj?.ExecutionStatus).IsEqualTo("1");
        Check.That(obj?.NumeroFluxDepot).IsEqualTo(null);
        Check.That(obj?.Errors).HasSize(1);
        Check.That(obj?.Errors.Select(x => x.Message)).ContainsExactly("Fisierul transmis nu este valid. org.xml.sax.SAXParseException; lineNumber: 1; columnNumber: 540; cvc-elt.1.a: Cannot find the declaration of element 'Invoice'.");
    }

    [TestMethod]
    public void Deserialize_Ok_Errors()
    {
        var xml = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<header xmlns=""mfp:anaf:dgti:spv:respUploadFisier:v1"" dateResponse=""202503312232"" ExecutionStatus=""1"">
    <Errors errorMessage=""Fisierul transmis nu este valid. org.xml.sax.SAXParseException; lineNumber: 1; columnNumber: 540; cvc-elt.1.a: Cannot find the declaration of element 'Invoice'.""/>
    <Errors errorMessage=""Autre erreur détectée dans le fichier transmis.""/>
</header>";
        var obj = XmlHelper.Deserialize<DepotXml>(xml);
        Check.That(obj).IsNotNull();
        Check.That(obj?.DateResponse).IsEqualTo("202503312232");
        Check.That(obj?.ExecutionStatus).IsEqualTo("1");
        Check.That(obj?.NumeroFluxDepot).IsEqualTo(null);
        Check.That(obj?.Errors).HasSize(2);
        Check.That(obj?.Errors.Select(x => x.Message)).ContainsExactly("Fisierul transmis nu este valid. org.xml.sax.SAXParseException; lineNumber: 1; columnNumber: 540; cvc-elt.1.a: Cannot find the declaration of element 'Invoice'.", "Autre erreur détectée dans le fichier transmis.");
    }
}