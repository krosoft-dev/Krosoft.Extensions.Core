using Newtonsoft.Json.Converters;

namespace Krosoft.Extensions.Core.Converters;

public class DateFrConverter : IsoDateTimeConverter
{
    public DateFrConverter()
    {
        DateTimeFormat = "dd/MM/yyyy";
    }
}