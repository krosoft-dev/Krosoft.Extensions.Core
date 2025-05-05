namespace Krosoft.Extensions.Core.Interfaces;

public interface IJsonService
{
    T? Deserialize<T>(string jsonString);
    string Serialize<T>(T obj);
}