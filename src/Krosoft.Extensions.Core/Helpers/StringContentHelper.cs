using System.Text;
using Krosoft.Extensions.Core.Extensions;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Helpers;

public static class StringContentHelper
{
    public static HttpContent SerializeAsNewtonsoftJson(object? data)
        => new StringContent(JsonConvert.SerializeObject(data),
                             Encoding.UTF8, 
                             HttpClientExtensions.MediaTypeJson);
}