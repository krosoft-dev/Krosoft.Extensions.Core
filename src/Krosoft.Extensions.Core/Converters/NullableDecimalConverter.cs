using System.Globalization;
using Krosoft.Extensions.Core.Extensions;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Converters;

public class NullableDecimalConverter : JsonConverter<decimal?>
{
    private readonly CultureInfo _culture;
    private readonly char[] _separators = { ',', '.' };

    public NullableDecimalConverter() : this(null)
    {
    }

    public NullableDecimalConverter(CultureInfo? culture)
    {
        if (culture == null)
        {
            _culture = CultureInfo.InvariantCulture;
        }
        else
        {
            _culture = culture;
        }
    }

    public override decimal? ReadJson(JsonReader reader, Type objectType, decimal? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.Value != null)
        {
            var input = reader.Value.ToString();
            if (input != null)
            {
                var value = new string(input.Where(c => char.IsDigit(c) || _separators.Contains(c))
                                            .ToArray()).Trim()
                                                       .Replace(_separators, _culture.NumberFormat.NumberDecimalSeparator);

                if (!string.IsNullOrWhiteSpace(value))
                {
                    return Convert.ToDecimal(value, _culture);
                }
            }
        }

        return null;
    }

    public override void WriteJson(JsonWriter writer, decimal? value, JsonSerializer serializer)
    {
        writer.WriteValue(value);
    }
}