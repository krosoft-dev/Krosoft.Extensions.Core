using System.Net.Http.Headers;
using Krosoft.Extensions.Core.Helpers;

namespace Krosoft.Extensions.Core.Extensions;

public static class HttpClientExtensions
{
    public const string MediaTypeJson = "application/json";

    /// <summary>
    /// Default value for AuthenticationScheme property in the JwtBearerAuthenticationOptions
    /// </summary>
    public const string JwtAuthenticationScheme = "Bearer";

    public static async Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client,
                                                                     string requestUri,
                                                                     T data)
        => await client.PostAsync(requestUri, StringContentHelper.SerializeAsJson(data));

    public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, string requestUri, T data)
        => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = StringContentHelper.SerializeAsJson(data) });

    public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient,
                                                                 string requestUri,
                                                                 T data,
                                                                 CancellationToken cancellationToken)
        => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = StringContentHelper.SerializeAsJson(data) }, cancellationToken);

    public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient,
                                                                 Uri requestUri,
                                                                 T data)
        => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = StringContentHelper.SerializeAsJson(data) });

    public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient,
                                                                 Uri requestUri,
                                                                 T data,
                                                                 CancellationToken cancellationToken)
        => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = StringContentHelper.SerializeAsJson(data) }, cancellationToken);

    public static Task<HttpResponseMessage> GetAsync<T>(this HttpClient httpClient,
                                                        string requestUri,
                                                        T data,
                                                        CancellationToken cancellationToken)
        => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUri) { Content = StringContentHelper.SerializeAsJson(data) }, cancellationToken);

    public static HttpClient SetBearerToken(this HttpClient httpClient,
                                            string token) =>
        httpClient.SetToken(JwtAuthenticationScheme, token);

    public static HttpClient SetToken(this HttpClient httpClient,
                                      string scheme,
                                      string token)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);
        return httpClient;
    }

    public static HttpClient SetHeader(this HttpClient httpClient,
                                       string name,
                                       string value,
                                       bool clear = false)
    {
        if (clear)
        {
            httpClient.DefaultRequestHeaders.Clear();
        }

        httpClient.DefaultRequestHeaders.Add(name, value);
        return httpClient;
    }
}