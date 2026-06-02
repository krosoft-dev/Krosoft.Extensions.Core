using Krosoft.Extensions.Core.Helpers;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class UrlHelperTests
{
    [TestMethod]
    public void GetUrl_SimpleComplement_Ok()
    {
        var result = UrlHelper.GetUrl("https://example.com/", "api/users");

        Check.That(result).IsEqualTo("https://example.com/api/users");
    }

    [TestMethod]
    public void GetUrl_BaseWithoutTrailingSlash_Ok()
    {
        var result = UrlHelper.GetUrl("https://example.com", "api/users");

        Check.That(result).IsEqualTo("https://example.com/api/users");
    }

    [TestMethod]
    public void GetUrl_ComplementWithLeadingSlash_Ok()
    {
        var result = UrlHelper.GetUrl("https://example.com/", "/api/users");

        Check.That(result).IsEqualTo("https://example.com/api/users");
    }

    [TestMethod]
    public void GetUrl_BaseWithPath_ComplementReplacesPath()
    {
        var result = UrlHelper.GetUrl("https://example.com/v1/", "api/users");

        Check.That(result).IsEqualTo("https://example.com/v1/api/users");
    }

    [TestMethod]
    public void GetUrl_EmptyComplement_ReturnsBase()
    {
        var result = UrlHelper.GetUrl("https://example.com/", "");

        Check.That(result).IsEqualTo("https://example.com/");
    }
}
