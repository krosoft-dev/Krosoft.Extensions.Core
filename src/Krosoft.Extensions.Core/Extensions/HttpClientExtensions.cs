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

    public static async Task<HttpResponseMessage> PostAsNewtonsoftJsonAsync<T>(this HttpClient client,
                                                                               string requestUri,
                                                                               T data,
                                                                               CancellationToken cancellationToken = default)
        => await client.PostAsync(requestUri, StringContentHelper.SerializeAsNewtonsoftJson(data), cancellationToken);

    public static Task<HttpResponseMessage> DeleteAsNewtonsoftJsonAsync<T>(this HttpClient httpClient,
                                                                           string requestUri,
                                                                           T data,
                                                                           CancellationToken cancellationToken = default)
        => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri)
        {
            Content = StringContentHelper.SerializeAsNewtonsoftJson(data)
        }, cancellationToken);

    public static Task<HttpResponseMessage> DeleteAsNewtonsoftJsonAsync<T>(this HttpClient httpClient,
                                                                           Uri requestUri,
                                                                           T data,
                                                                           CancellationToken cancellationToken = default)
        => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri)
        {
            Content = StringContentHelper.SerializeAsNewtonsoftJson(data)
        }, cancellationToken);

    public static Task<HttpResponseMessage> GetAsNewtonsoftAsync<T>(this HttpClient httpClient,
                                                                    string requestUri,
                                                                    T data,
                                                                    CancellationToken cancellationToken = default)
        => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUri)
        {
            Content = StringContentHelper.SerializeAsNewtonsoftJson(data)
        }, cancellationToken);

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

    /// <summary>
    /// Add (or update) the header with the specified <paramref name="name" />.
    /// </summary>
    /// <param name="client">Collection of headers.</param>
    /// <param name="name">name of the header to add./update</param>
    /// <param name="value">new value of the header.</param>
    /// <remarks>
    /// The header's value will be updated if already present in  <see cref="HttpClient.DefaultRequestHeaders" />.
    /// </remarks>
    public static HttpClient AddOrUpdateHeader(this HttpClient client, string name, string value)
    {
        client.DefaultRequestHeaders.Remove(name);
        client.DefaultRequestHeaders.TryAddWithoutValidation(name, value);

        return client;
    }
}