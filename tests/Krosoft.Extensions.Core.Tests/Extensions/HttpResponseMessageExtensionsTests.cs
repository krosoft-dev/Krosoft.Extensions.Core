using System.Net;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Models.Exceptions.Http;
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
    public void EnsureAsync_NotSuccessStatusCode_HttpExceptionThrown()
    {
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest);

        Check.ThatCode(async () => await response.EnsureAsync()).Throws<HttpException>();
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
}