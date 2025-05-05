using Krosoft.Extensions.Core.Interfaces;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Services;

public class JsonService : IJsonService
{
    public T? Deserialize<T>(string jsonString)
    {
        var obj = JsonConvert.DeserializeObject<T>(jsonString);
        return obj;
    }

    public string Serialize<T>(T obj)
    {
        var jsonString = JsonConvert.SerializeObject(obj);
        return jsonString;
    }
}