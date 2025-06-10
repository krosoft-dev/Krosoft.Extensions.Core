﻿using Krosoft.Extensions.Core.Models.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Krosoft.Extensions.Core.Converters;

public class KrosoftTechnicalExceptionConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => objectType == typeof(KrosoftTechnicalException);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.StartObject)
        {
            var properties = new Dictionary<string, object>();
            serializer.Populate(reader, properties);
            var array = properties.GetValueOrDefault(nameof(KrosoftTechnicalException.Errors)) as JArray;
            var errors = new HashSet<string>();
            if (array != null)
            {
                var o = array.ToObject<List<string>>();
                if (o == null)
                {
                    throw new InvalidOperationException();
                }

                errors = o.ToHashSet();
            }

            return new KrosoftTechnicalException(errors);
        }

        return null;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}