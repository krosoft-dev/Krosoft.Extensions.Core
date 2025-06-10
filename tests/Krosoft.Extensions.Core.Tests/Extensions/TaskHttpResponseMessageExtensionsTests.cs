using System.Net;
using System.Net.Http.Headers;
using JetBrains.Annotations;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Models.Exceptions.Http;
using Krosoft.Extensions.Samples.Library.Models.Xml;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
[TestSubject(typeof(TaskHttpResponseMessageExtensions))]
public class TaskHttpResponseMessageExtensionsTests
{
    [TestMethod]
    public void EnsureAsync_SuccessStatusCode_NoExceptionThrown()
    {
        var response = Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));

        Check.ThatCode(async () => await response.EnsureAsync()).DoesNotThrow();
    }

    [TestMethod]
    public void EnsureAsync_BadRequest_ThrowsException()
    {
        var responseObject = new
        {
            Code = 400,
            Message = "BadRequest",
            Errors = new List<string> { "Validation error" }
        };
        var json = JsonConvert.SerializeObject(responseObject);
        var response = Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(json)
        });

        Check.ThatCode(async () => await response.EnsureAsync())
             .Throws<KrosoftFunctionalException>()
             .WithMessage("Validation error");
    }

    [TestMethod]
    public async Task EnsureAsync_Generic_SuccessStatusCode_ReturnsValue()
    {
        var responseObject = new { Message = "Success" };
        var json = JsonConvert.SerializeObject(responseObject);
        var response = Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json)
        });

        var result = await response.EnsureAsync<dynamic>();

        Check.That(result?.Message).Equals("Success");
    }

    [TestMethod]
    public async Task EnsureStringAsync_SuccessStatusCode_ReturnsContent()
    {
        var content = "test content";
        var response = Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(content)
        });

        var result = await response.EnsureStringAsync();

        Check.That(result).Equals(content);
    }

    [TestMethod]
    public async Task EnsureStreamAsync_SuccessStatusCode_ReturnsStream()
    {
        var response = Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("test content")
        });
        response.Result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        response.Result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
        {
            FileName = "test.txt"
        };

        var result = await response.EnsureStreamAsync();

        Check.That(result).IsNotNull();
        Check.That(result?.FileName).Equals("test.txt");
        Check.That(result?.ContentType).Contains("text/plain");
    }

    [TestMethod]
    public async Task EnsureResultAsync_Generic_SuccessStatusCode_ReturnsSuccessResult()
    {
        var responseObject = new { Message = "Success" };
        var json = JsonConvert.SerializeObject(responseObject);
        var response = Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json)
        });

        var result = await response.EnsureResultAsync<dynamic>();

        Check.That(result.IsSuccess).IsTrue();
        Check.That(result.Value?.Message).Equals("Success");
    }

    [TestMethod]
    public async Task EnsureXmlAsync_SuccessStatusCode_ReturnsDeserializedObject()
    {
        var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><header xmlns=\"mfp:anaf:dgti:spv:respUploadFisier:v1\" dateResponse=\"202503312230\" ExecutionStatus=\"0\" index_incarcare=\"5020282769\"/>";
        var response = Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(xml)
        });

        var result = await response.EnsureXmlAsync<DepotXml>();

        Check.That(result).IsNotNull();
        Check.That(result?.DateResponse).Equals("202503312230");
        Check.That(result?.ExecutionStatus).Equals("0");
        Check.That(result?.NumeroFluxDepot).Equals("5020282769");
    }

    [TestMethod]
    public async Task EnsureResultXmlAsync_SuccessStatusCode_ReturnsSuccessResult()
    {
        var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><header xmlns=\"mfp:anaf:dgti:spv:respUploadFisier:v1\" dateResponse=\"202503312230\" ExecutionStatus=\"0\" index_incarcare=\"5020282769\"/>";
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(xml)
        };
        var task = Task.FromResult(response);

        var result = await task.EnsureResultXmlAsync<DepotXml>();

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
        var task = Task.FromResult(response);

        Check.ThatCode(async () => await task.EnsureResultXmlAsync<DepotXml>())
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
        var task = Task.FromResult(response);

        var result = await task.EnsureResultStreamAsync();

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
        var task = Task.FromResult(response);

        Check.ThatCode(async () => await task.EnsureResultStreamAsync())
             .Throws<HttpException>()
             .WithMessage("Not Found")
             .And.WithProperty(x => x.Code, HttpStatusCode.NotFound);
    }

    [TestMethod]
    public void EnsureResultAsync_SuccessStatusCode_NoContent_NoExceptionThrown()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        var task = Task.FromResult(response);

        Check.ThatCode(async () => await task.EnsureResultAsync()).DoesNotThrow();
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
        var task = Task.FromResult(response);

        Check.ThatCode(async () => await task.EnsureResultAsync())
             .Throws<KrosoftFunctionalException>()
             .WithMessage("Validation failed")
             .And.WithProperty(x => x.Code, HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public async Task EnsureResultStringAsync_SuccessStatusCode_ReturnsSuccessResult()
    {
        var content = "test content";
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(content)
        };
        var task = Task.FromResult(response);

        var result = await task.EnsureResultStringAsync();

        Check.That(result.IsSuccess).IsTrue();
        Check.That(result.Value).Equals(content);
    }

    [TestMethod]
    public void EnsureResultStringAsync_ErrorStatusCode_ThrowsException()
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
        var task = Task.FromResult(response);

        Check.ThatCode(async () => await task.EnsureResultStringAsync())
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
        var task = Task.FromResult(response);

        var result = await task.EnsureResultStringAsync();

        Check.That(result.IsSuccess).IsTrue();
        Check.That(result.Value).IsEmpty();
    }
}