using System.Net;
using System.Net.Http.Headers;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Models.Exceptions.Http;
using Krosoft.Extensions.Samples.Library.Models;
using Krosoft.Extensions.Testing;
using Moq.Protected;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class HttpClientExtensionsTests : BaseTest
{
    [TestMethod]
    public async Task DeleteAsJsonAsync_SendsDeleteRequestWithJsonContent()
    {
        // Arrange
        var requestData = new { Name = "Test" };
        var requestUri = "https://example.com/resource";

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        httpMessageHandlerMock.Protected()
                              .Setup<Task<HttpResponseMessage>>(
                                                                "SendAsync",
                                                                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri != null && req.Method == HttpMethod.Delete && req.RequestUri.ToString() == requestUri),
                                                                ItExpr.IsAny<CancellationToken>()
                                                               )
                              .ReturnsAsync(httpResponseMessage);

        var httpClient = new HttpClient(httpMessageHandlerMock.Object);

        // Act
        var response = await httpClient.DeleteAsJsonAsync(requestUri, requestData);

        // Assert
        Check.That(response).IsNotNull();
        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

        httpMessageHandlerMock.Protected()
                              .Verify(
                                      "SendAsync",
                                      Times.Once(),
                                      ItExpr.Is<HttpRequestMessage>(req =>
                                                                        req.Content != null &&
                                                                        req.Content.Headers.ContentType != null &&
                                                                        req.Content != null &&
                                                                        req.RequestUri != null &&
                                                                        req.Method == HttpMethod.Delete &&
                                                                        req.RequestUri.ToString() == requestUri &&
                                                                        req.Content.Headers.ContentType.MediaType == HttpClientExtensions.MediaTypeJson),
                                      ItExpr.IsAny<CancellationToken>()
                                     );
    }

    [TestMethod]
    public async Task EnsureAsync()
    {
        var cancellationToken = CancellationToken.None;

        var result = await new HttpClient()
                           .GetAsync("https://jsonplaceholder.typicode.com/todos", cancellationToken)
                           .EnsureAsync<IEnumerable<TodoHttp>>((_, json) => throw new KrosoftTechnicalException(json), cancellationToken)
                           .ToList();

        Check.That(result).IsNotNull();
        Check.That(result).HasSize(200);
    }

    [TestMethod]
    public void EnsureAsync_Exception_Custom()
    {
        var cancellationToken = CancellationToken.None;

        Check.ThatCode(() => new HttpClient()
                             .PutAsync("https://jsonplaceholder.typicode.com/posts", null, cancellationToken)
                             .EnsureAsync<IEnumerable<TodoHttp>>((_, json) => throw new HttpException(HttpStatusCode.InternalServerError, json), cancellationToken))
             .Throws<HttpException>()
             .WithMessage("{}")
             .And.WhichMember(x => x.Code)
             .IsEqualTo(HttpStatusCode.InternalServerError);
    }

    [TestMethod]
    public void EnsureAsync_Exception_Default()
    {
        var cancellationToken = CancellationToken.None;

        Check.ThatCode(() => new HttpClient()
                             .PutAsync("https://jsonplaceholder.typicode.com/posts", null, cancellationToken)
                             .EnsureAsync<IEnumerable<TodoHttp>>(cancellationToken))
             .Throws<HttpException>()
             .WithMessage("Not Found")
             .And.WhichMember(x => x.Code)
             .IsEqualTo(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task EnsureAsync_NoReturn()
    {
        var cancellationToken = CancellationToken.None;

        await new HttpClient()
              .GetAsync("https://jsonplaceholder.typicode.com/todos", cancellationToken)
              .EnsureAsync(cancellationToken);
    }

    [TestMethod]
    public async Task EnsureAsync_NoReturn_OnError()
    {
        var cancellationToken = CancellationToken.None;

        await new HttpClient()
              .GetAsync("https://jsonplaceholder.typicode.com/todos", cancellationToken)
              .EnsureAsync((_, json) => throw new KrosoftTechnicalException(json), cancellationToken);
    }

    [TestMethod]
    public void EnsureStreamAsync_Exception()
    {
        var cancellationToken = CancellationToken.None;

        var mockHttp = GetMock(HttpStatusCode.NotFound, null, null, null);

        Check.ThatCode(() => mockHttp
                             .GetAsync("https://example.com", cancellationToken)
                             .EnsureStreamAsync(cancellationToken))
             .Throws<HttpException>()
             .WithMessage("Not Found")
             .And.WhichMember(x => x.Code)
             .IsEqualTo(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public void EnsureStreamAsync_Exception_OnError()
    {
        var cancellationToken = CancellationToken.None;

        var mockHttp = GetMock(HttpStatusCode.NotFound, null, null, null);

        Check.ThatCode(() => mockHttp
                             .GetAsync("https://example.com", cancellationToken)
                             .EnsureStreamAsync((code, json) => throw new HttpException(code, json), cancellationToken))
             .Throws<HttpException>()
             .WithMessage("Not Found")
             .And.WhichMember(x => x.Code)
             .IsEqualTo(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task EnsureStreamAsync_Ok()
    {
        var content = "Test content";
        var contentType = HttpClientExtensions.MediaTypeJson;
        var fileName = "test.json";

        var cancellationToken = CancellationToken.None;

        var mockHttp = GetMock(HttpStatusCode.OK, content, fileName, contentType);

        var result = await mockHttp
                           .GetAsync("https://example.com", cancellationToken)
                           .EnsureStreamAsync(cancellationToken);

        Check.That(result).IsNotNull();
        Check.That(result!.ContentType).IsEqualTo(contentType);
        Check.That(result.FileName).IsEqualTo(fileName);

        using var reader = new StreamReader(result.Stream);
        var actualContent = await reader.ReadToEndAsync(cancellationToken);
        Check.That(actualContent).IsEqualTo(content);
    }

    [TestMethod]
    public async Task EnsureStreamAsync_Ok_OnError()
    {
        var content = "Test content";
        var contentType = HttpClientExtensions.MediaTypeJson;
        var fileName = "test.json";

        var cancellationToken = CancellationToken.None;

        var mockHttp = GetMock(HttpStatusCode.OK, content, fileName, contentType);

        var result = await mockHttp
                           .GetAsync("https://example.com", cancellationToken)
                           .EnsureStreamAsync((code, json) => throw new HttpException(code, json), cancellationToken);

        Check.That(result).IsNotNull();
        Check.That(result!.ContentType).IsEqualTo(contentType);
        Check.That(result.FileName).IsEqualTo(fileName);

        using var reader = new StreamReader(result.Stream);
        var actualContent = await reader.ReadToEndAsync(cancellationToken);
        Check.That(actualContent).IsEqualTo(content);
    }

    [TestMethod]
    public void EnsureStringAsync_Exception()
    {
        var mockHttp = GetMock(HttpStatusCode.NotFound, null, null, null);

        Check.ThatCode(() => mockHttp.GetAsync("https://example.com")
                                     .EnsureStringAsync(CancellationToken.None))
             .Throws<HttpException>()
             .WithMessage("Not Found")
             .And.WhichMember(x => x.Code)
             .IsEqualTo(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task EnsureStringAsync_Ok()
    {
        var cancellationToken = CancellationToken.None;
        var content = "Test content";

        var mockHttp = GetMock(HttpStatusCode.OK, content, null, null);

        var result = await mockHttp.GetAsync("https://example.com", cancellationToken)
                                   .EnsureStringAsync(cancellationToken);

        Check.That(result).IsEqualTo(content);
    }

    private static HttpClient GetMock(HttpStatusCode statusCode,
                                      string? content,
                                      string? fileName,
                                      string? contentType)
    {
        var httpResponseMessage = new HttpResponseMessage(statusCode);
        if (content != null)
        {
            httpResponseMessage.Content = new StringContent(content);
            if (fileName != null)
            {
                httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };
            }

            if (contentType != null)
            {
                httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            }
        }

        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        httpMessageHandlerMock.Protected()
                              .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                              .ReturnsAsync(httpResponseMessage);

        return new HttpClient(httpMessageHandlerMock.Object);
    }

    [TestMethod]
    public void SetBearerToken_Ok()
    {
        var token = "test-token";
        var httpClient = new HttpClient().SetBearerToken(token);

        var authHeader = httpClient.DefaultRequestHeaders.Authorization;
        Check.That(authHeader).IsNotNull();
        Check.That(authHeader?.Scheme).IsEqualTo(HttpClientExtensions.JwtAuthenticationScheme);
        Check.That(authHeader?.Parameter).IsEqualTo(token);
    }

    [TestMethod]
    public void SetToken_Ok()
    {
        var scheme = "api";
        var token = "test-token";
        var httpClient = new HttpClient().SetToken(scheme, token);

        var authHeader = httpClient.DefaultRequestHeaders.Authorization;
        Check.That(authHeader).IsNotNull();
        Check.That(authHeader?.Scheme).IsEqualTo(scheme);
        Check.That(authHeader?.Parameter).IsEqualTo(token);
    }

    [TestMethod]
    public void SetHeader_Ok()
    {
        var scheme = "api";
        var token = "test-token";
        var httpClient = new HttpClient().SetHeader(scheme, token).SetHeader(scheme, token);

        var headers = httpClient.DefaultRequestHeaders.ToDictionary(x => x.Key, x => x.Value);
        Check.That(headers).IsNotNull();
        Check.That(headers).HasSize(1);
        Check.That(headers[scheme]).IsEqualTo(new List<string> { token, token });
    }

    [TestMethod]
    public void SetHeader_Multiple_Ok()
    {
        var scheme = "api";
        var token = "test-token";
        var httpClient = new HttpClient().SetHeader(scheme, token, true).SetHeader(scheme, token, true);

        var headers = httpClient.DefaultRequestHeaders.ToDictionary(x => x.Key, x => x.Value);
        Check.That(headers).IsNotNull();
        Check.That(headers).HasSize(1);
        Check.That(headers[scheme]).IsEqualTo(new List<string> { token });
    }
}