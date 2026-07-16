using System.Reflection;
using Krosoft.Extensions.Core.Extensions;
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

    private static MemoryStream GetStream(string fileName)
    {
        var bytes = Assembly.GetExecutingAssembly().Read(fileName).ReadAsByteArray();
        return new MemoryStream(bytes);
    }

    [TestMethod]
    public void Load_Ok()
    {
        using var stream = GetStream("xml.xml");
        var result = XmlHelper.Load(stream, "root");

        Check.That(result.Exception).IsNull();
        Check.That(result.Value).IsNotNull();
        Check.That(result.Value!.DocumentElement!.LocalName).IsEqualTo("root");
    }

    [TestMethod]
    public void Load_Ok_Sbdh()
    {
        using var stream = GetStream("sample_2.xml");
        var result = XmlHelper.Load(stream, "StandardBusinessDocument");

        Check.That(result.Exception).IsNull();
        Check.That(result.Value).IsNotNull();
        Check.That(result.Value!.DocumentElement!.LocalName).IsEqualTo("StandardBusinessDocument");
    }

    [TestMethod]
    public void Load_Ok_Ubl()
    {
        using var stream = GetStream("sample_ubl_ko.xml");
        var result = XmlHelper.Load(stream, "Invoice");

        Check.That(result.Exception).IsNull();
        Check.That(result.Value).IsNotNull();
        Check.That(result.Value!.DocumentElement!.LocalName).IsEqualTo("Invoice");
    }

    [TestMethod]
    public void Load_Ok_Bom()
    {
        using var stream = GetStream("XML_UTF8_BOM.xml");
        var result = XmlHelper.Load(stream, "CrossIndustryInvoice");

        Check.That(result.Exception).IsNull();
        Check.That(result.Value).IsNotNull();
        Check.That(result.Value!.DocumentElement!.LocalName).IsEqualTo("CrossIndustryInvoice");
    }

    [TestMethod]
    public void Load_Wrong_Root()
    {
        using var stream = GetStream("xml.xml");
        var result = XmlHelper.Load(stream, "Invoice");

        Check.That(result.Value).IsNull();
        Check.That(result.Exception).IsNotNull();
        Check.That(result.Exception!.Message).IsEqualTo("Le fichier n'est pas valide (élément racine attendu : 'Invoice', trouvé : 'root').");
    }

    [TestMethod]
    public void Load_Invalid_Content()
    {
        using var stream = GetStream("invalide.txt");
        var result = XmlHelper.Load(stream, "root");

        Check.That(result.Value).IsNull();
        Check.That(result.Exception).IsNotNull();
        Check.That(result.Exception!.Message).StartsWith("Le fichier 'root' n'a pas pu être lu : ");
    }
}