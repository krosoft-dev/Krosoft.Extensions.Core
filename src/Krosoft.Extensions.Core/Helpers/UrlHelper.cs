namespace Krosoft.Extensions.Core.Helpers;

public static class UrlHelper
{
    public static string GetUrl(string baseUrl, string complementUrl)
    {
        var baseUri = new Uri(baseUrl);
        var myUri = new Uri(baseUri, complementUrl);
        var url = myUri.ToString();
        return url;
    }
}