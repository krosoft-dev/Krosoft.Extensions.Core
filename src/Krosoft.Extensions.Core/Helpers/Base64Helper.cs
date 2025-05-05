using System.Text;
using System.Text.RegularExpressions;

namespace Krosoft.Extensions.Core.Helpers;

public static class Base64Helper
{
    /// <summary>
    /// Transforme un texte en base 64 en string
    /// </summary>
    /// <param name="base64EncodedData">texte en base 64</param>
    /// <returns>le texte en string</returns>
    public static string? Base64ToString(string? base64EncodedData)
    {
        if (base64EncodedData != null)
        {
            var encodedDataAsBytes = Convert.FromBase64String(base64EncodedData);
            var plainText = Encoding.UTF8.GetString(encodedDataAsBytes, 0, encodedDataAsBytes.Length);
            return plainText;
        }

        return null;
    }

    public static bool IsBase64String(string base64)
    {
        if (string.IsNullOrEmpty(base64))
        {
            return false;
        }

        // Vérifier si la longueur est multiple de 4.
        if (base64.Length % 4 != 0)
        {
            return false;
        }

        // Vérifier les caractères valides.
        var base64Regex = new Regex(@"^[a-zA-Z0-9\+/]*={0,2}$", RegexOptions.None, RegexHelper.MatchTimeout);
        if (!base64Regex.IsMatch(base64))
        {
            return false;
        }

        return true;
    }

    public static string? StringToBase64(string? plainText)
    {
        if (plainText != null)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        return null;
    }
}