using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Extensions;

public static class HttpContentExtensions
{
    public static async Task<T?> ReadAsJsonAsync<T>(this HttpContent content,
                                                    CancellationToken cancellationToken = default)
    {
        var json = await content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<T>(json);
    }
}