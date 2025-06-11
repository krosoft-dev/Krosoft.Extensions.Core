using System.Net;
using System.Net.Http.Headers;
using JetBrains.Annotations;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Models.Dto;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Models.Exceptions.Http;
using Krosoft.Extensions.Samples.Library.Models.Xml;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
[TestSubject(typeof(HttpResponseMessageExtensions))]
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
             .WithMessage("Bad Request")
             .And.WithProperty(x => x.Code, HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public void EnsureAsync_InternalServerError()
    {
        var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

        Check.ThatCode(async () => await response.EnsureAsync())
             .Throws<HttpException>()
             .WithMessage("Internal Server Error")
             .And.WithProperty(x => x.Code, HttpStatusCode.InternalServerError);
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

    [TestMethod]
    public async Task EnsureResultAsync_Generic_SuccessStatusCode_ReturnsSuccessResult()
    {
        var responseObject = new { Message = "Success" };
        var json = JsonConvert.SerializeObject(responseObject);
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json)
        };

        var result = await response.EnsureResultAsync<dynamic>();

        Check.That(result.IsSuccess).IsTrue();
        Check.That(result.Value?.Message).Equals("Success");
    }

    [TestMethod]
    public async Task EnsureResultAsync_BadRequest_ReturnsFailureResult()
    {
        var responseObject = new
        {
            Code = 400,
            Message = "BadRequest",
            Errors = new List<string> { "Validation error" }
        };
        var json = JsonConvert.SerializeObject(responseObject);
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(json)
        };

        var result = await response.EnsureResultAsync<dynamic>();

        Check.That(result.IsFaulted).IsTrue();
        Check.That(result.Exception).IsInstanceOf<KrosoftFunctionalException>();
    }

    [TestMethod]
    public async Task EnsureStreamAsync_SuccessStatusCode_ReturnsStream()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("test content")
        };
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
        {
            FileName = "test.txt"
        };

        var result = await response.EnsureStreamAsync(null);

        Check.That(result).IsNotNull();
        Check.That(result?.FileName).Equals("test.txt");
        Check.That(result?.ContentType).Contains("text/plain");
    }

    [TestMethod]
    public async Task EnsureResultStringAsync_SuccessStatusCode_ReturnsSuccessResult()
    {
        var content = "test content";
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(content)
        };

        var result = await response.EnsureResultStringAsync();

        Check.That(result.IsSuccess).IsTrue();
        Check.That(result.Value).Equals(content);
    }

    [TestMethod]
    public void EnsureResultStringAsync_ErrorStatusCode_ReturnsFailureResult()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent("Not Found")
        };

        Check.ThatCode(async () => await response.EnsureResultStringAsync())
             .Throws<HttpException>()
             .WithMessage("Not Found")
             .And.WithProperty(x => x.Code, HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task EnsureResultXmlAsync_SuccessStatusCode_ReturnsSuccessResult()
    {
        var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><header xmlns=\"mfp:anaf:dgti:spv:respUploadFisier:v1\" dateResponse=\"202503312230\" ExecutionStatus=\"0\" index_incarcare=\"5020282769\"/>";
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(xml)
        };

        var result = await response.EnsureResultXmlAsync<DepotXml>();

        Check.That(result.IsSuccess).IsTrue();
        Check.That(result.Value).IsNotNull();
        Check.That(result.Value?.DateResponse).Equals("202503312230");
        Check.That(result.Value?.ExecutionStatus).Equals("0");
        Check.That(result.Value?.NumeroFluxDepot).Equals("5020282769");
    }

    [TestMethod]
    public void EnsureResultXmlAsync_ErrorStatusCode_ReturnsFailureResult()
    {
        var responseObject = new
        {
            Code = 400,
            Message = "BadRequest",
            Errors = new List<string> { "Invalid XML" }
        };
        var json = JsonConvert.SerializeObject(responseObject);
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(json)
        };

        Check.ThatCode(async () => await response.EnsureResultXmlAsync<DepotXml>())
             .Throws<KrosoftFunctionalException>()
             .WithMessage("Invalid XML")
             .And.WithProperty(x => x.Code, HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public async Task EnsureResultStreamAsync_SuccessStatusCode_ReturnsSuccessResult()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("test content")
        };
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
        {
            FileName = "test.txt"
        };

        var result = await response.EnsureResultStreamAsync();

        Check.That(result.IsSuccess).IsTrue();
        Check.That(result.Value).IsNotNull();
        Check.That(result.Value?.FileName).Equals("test.txt");
        Check.That(result.Value?.ContentType).Contains("text/plain");
    }

    [TestMethod]
    public void EnsureResultStreamAsync_ErrorStatusCode_ReturnsFailureResult()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent("Not Found")
        };

        Check.ThatCode(async () => await response.EnsureResultStreamAsync())
             .Throws<HttpException>()
             .WithMessage("Not Found")
             .And.WithProperty(x => x.Code, HttpStatusCode.NotFound);
    }

    [TestMethod]
    public void EnsureResultAsync_SuccessStatusCode_NoContent_NoExceptionThrown()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        Check.ThatCode(async () => await response.EnsureResultAsync()).DoesNotThrow();
    }

    [TestMethod]
    public void EnsureResultAsync_ErrorStatusCode_ThrowsException()
    {
        var responseObject = new
        {
            Code = 400,
            Message = "BadRequest",
            Errors = new List<string> { "Validation failed" }
        };
        var json = JsonConvert.SerializeObject(responseObject);
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(json)
        };

        Check.ThatCode(async () => await response.EnsureResultAsync())
             .Throws<KrosoftFunctionalException>()
             .WithMessage("Validation failed")
             .And.WithProperty(x => x.Code, HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public void EnsureResultStringAsync_ErrorWithJsonError_ThrowsException()
    {
        var responseObject = new
        {
            Code = 400,
            Message = "Validation Error",
            Errors = new List<string> { "Field is required" }
        };
        var json = JsonConvert.SerializeObject(responseObject);
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(json)
        };

        Check.ThatCode(async () => await response.EnsureResultStringAsync())
             .Throws<KrosoftFunctionalException>()
             .WithMessage("Field is required")
             .And.WithProperty(x => x.Code, HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public async Task EnsureResultStringAsync_EmptyContent_ReturnsEmptyString()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(string.Empty)
        };

        var result = await response.EnsureResultStringAsync();

        Check.That(result.IsSuccess).IsTrue();
        Check.That(result.Value).IsEmpty();
    }

    [TestMethod]
    public void EnsureAsync_ErrorDto_WithValidHttpStatusCode_ThrowsHttpException()
    {
        var responseObject = new ErrorDto
        {
            Code = 404,
            Message = "Resource not found"
        };
        var json = JsonConvert.SerializeObject(responseObject);
        var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent(json)
        };

        Check.ThatCode(async () => await response.EnsureAsync())
             .Throws<HttpException>()
             .WithMessage("Resource not found")
             .And.WithProperty(x => x.Code, HttpStatusCode.NotFound);
    }

    [TestMethod]
    public void EnsureAsync_ErrorDto_WithCustomCode_ThrowsHttpException()
    {
        var responseObject = new ErrorDto
        {
            Code = 499,
            Message = "Custom error message"
        };
        var json = JsonConvert.SerializeObject(responseObject);
        var response = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent(json)
        };

        Check.ThatCode(async () => await response.EnsureAsync())
             .Throws<HttpException>()
             .WithMessage("Custom error message")
             .And.WithProperty(x => x.Code, HttpStatusCode.NotFound);
    }

    [TestMethod]
    public void EnsureAsync_ErrorDto_WithZeroCode_ThrowsDefaultHttpException()
    {
        var responseObject = new ErrorDto
        {
            Code = 0,
            Message = "Zero code error"
        };
        var json = JsonConvert.SerializeObject(responseObject);
        var response = new HttpResponseMessage(HttpStatusCode.Gone)
        {
            Content = new StringContent(json)
        };

        Check.ThatCode(async () => await response.EnsureAsync())
             .Throws<HttpException>()
             .WithMessage("Gone")
             .And.WithProperty(x => x.Code, HttpStatusCode.Gone);
    }
}