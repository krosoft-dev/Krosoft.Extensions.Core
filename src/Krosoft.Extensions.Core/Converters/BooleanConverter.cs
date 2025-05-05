using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Converters;

public class BooleanConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => objectType == typeof(bool);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        switch (reader.Value?.ToString()?.ToLower().Trim())
        {
            case "vrai":
            case "true":
            case "yes":
            case "y":
            case "v":
            case "1":
                return true;
            case "faux":
            case "false":
            case "no":
            case "f":
            case "n":
            case "0":
                return false;

            case "null":
            case "na":

                return null;
        }

        // If we reach here, we're pretty much going to throw an error so let's let Json.NET throw it's pretty-fied error message.
        return new JsonSerializer().Deserialize(reader, objectType);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}