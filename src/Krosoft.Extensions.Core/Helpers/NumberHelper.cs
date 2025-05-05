using System.Globalization;
using System.Text.RegularExpressions;

namespace Krosoft.Extensions.Core.Helpers;

public static class NumberHelper
{
    public static decimal ToDecimal(string? text)
    {
        if (text != null)
        {
            var regex = new Regex(@"^-?\d+(?:\.\d+)?", RegexOptions.None, RegexHelper.MatchTimeout);
            var match = regex.Match(text);
            if (match.Success)
            {
                return decimal.Parse(match.Value, CultureInfo.InvariantCulture);
            }
        }

        return 0;
    }

    public static int ToInteger(string? text)
    {
        if (text != null)
        {
            var regex = new Regex(@"\d+", RegexOptions.None, RegexHelper.MatchTimeout);
            var match = regex.Match(text);
            if (match.Success)
            {
                return int.Parse(match.Value, CultureInfo.InvariantCulture);
            }
        }

        return 0;
    }
}