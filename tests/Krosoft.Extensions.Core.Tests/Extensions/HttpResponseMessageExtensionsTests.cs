using System.Net;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Models.Exceptions.Http;
using Krosoft.Extensions.Samples.Library.Models.Xml;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class HttpResponseMessageExtensionsTests
{
    [TestMethod]
    public void EnsureAsync_SuccessStatusCode_NoExceptionThrown()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        Check.ThatCode(async () => await response.EnsureAsync()).DoesNotThrow();
    }

    [TestMethod]
    public void EnsureAsync_BadRequest_Krosoft()
    {
        var responseObject = new
        {
            Code = 400,
            Message = "BadRequest",
            Errors = new List<string>
            {
                "Id requis !"
            }
        };
        var json = JsonConvert.SerializeObject(responseObject);
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(json)
        };

        Check.ThatCode(async () => await response.EnsureAsync())
             .Throws<KrosoftFunctionalException>()
             .WithMessage("Id requis !")
             .And.WithProperty(x => x.Code, HttpStatusCode.BadRequest)
             .And.WithProperty(x => x.Errors, new List<string> { "Id requis !" });
    }

    [TestMethod]
    public void EnsureAsync_InternalServerError_Krosoft()
    {
        var responseObject = new
        {
            Code = 500,
            Message = "InternalServerError",
            Errors = new List<string>
            {
                "Timeouts are not supported on this stream."
            }
        };
        var json = JsonConvert.SerializeObject(responseObject);
        var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent(json)
        };

        Check.ThatCode(async () => await response.EnsureAsync())
             .Throws<KrosoftTechnicalException>()
             .WithMessage("Timeouts are not supported on this stream.")
             .And.WithProperty(x => x.Code, HttpStatusCode.InternalServerError);
    }

    [TestMethod]
    public void EnsureAsync_BadRequest()
    {
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest);

        Check.ThatCode(async () => await response.EnsureAsync())
             .Throws<HttpException>()
             .WithMessage("Bad Request");
    }

    [TestMethod]
    public void EnsureAsync_InternalServerError()
    {
        var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

        Check.ThatCode(async () => await response.EnsureAsync())
             .Throws<HttpException>()
             .WithMessage("Internal Server Error");
    }

    [TestMethod]
    public async Task EnsureAsync_Generic_SuccessStatusCode_DeserializeObject()
    {
        var responseObject = new { Message = "Success" };
        var json = JsonConvert.SerializeObject(responseObject);
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json)
        };

        var result = await response.EnsureAsync<dynamic>();

        Check.That(result?.Message).Equals("Success");
    }

    [TestMethod]
    public async Task EnsureXmlAsync_Ok()
    {
        var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><header xmlns=\"mfp:anaf:dgti:spv:respUploadFisier:v1\" dateResponse=\"202503312230\" ExecutionStatus=\"0\" index_incarcare=\"5020282769\"/>";

        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(xml)
        };

        var depotXml = await response.EnsureXmlAsync<DepotXml>();

        Check.That(depotXml).IsNotNull();
        Check.That(depotXml?.DateResponse).Equals("202503312230");
        Check.That(depotXml?.ExecutionStatus).Equals("0");
        Check.That(depotXml?.NumeroFluxDepot).Equals("5020282769");
        Check.That(depotXml?.Errors).IsEmpty();
    }

    [TestMethod]
    public void EnsureXmlAsync_Ko()
    {
        var responseObject = new
        {
            Code = 400,
            Message = "BadRequest",
            Errors = new List<string>
            {
                "Id requis !"
            }
        };
        var json = JsonConvert.SerializeObject(responseObject);
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(json)
        };

        Check.ThatCode(async () => await response.EnsureXmlAsync<DepotXml>())
             .Throws<KrosoftFunctionalException>()
             .WithMessage("Id requis !")
             .And.WithProperty(x => x.Code, HttpStatusCode.BadRequest)
             .And.WithProperty(x => x.Errors, new List<string> { "Id requis !" });
    }
}