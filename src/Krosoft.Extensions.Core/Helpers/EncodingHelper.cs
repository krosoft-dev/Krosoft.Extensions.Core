using System.Text;
using Krosoft.Extensions.Core.Constantes;

namespace Krosoft.Extensions.Core.Helpers;

public static class EncodingHelper
{
    public static Encoding GetEuropeOccidentale() => Encoding.GetEncoding(StandardEncoding.EuropeOccidentale);
}