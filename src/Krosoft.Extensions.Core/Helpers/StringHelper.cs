using System.Text.RegularExpressions;

namespace Krosoft.Extensions.Core.Helpers;

/// <summary>
/// Méthodes d'aide pour les objets de type <see cref="string" />.
/// </summary>
public static class StringHelper
{
    public static string? ClearFilePath(string? source) =>
        source?.Replace(" ", "-")
              .Replace("/", "-");

    public static string FormatCurrency(decimal montant, string currencyIsoCode) => $"{currencyIsoCode} {FormatNumber(montant)}";

    public static string FormatDate(string? source)
    {
        DateTime.TryParse(source, out var date);
        return date.ToString("d");
    }

    public static string FormatNumber(decimal montant) => $"{montant:# ##0.00}".Replace('.', ',');

    public static Stream GenerateStreamFromString(string? source)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(source);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    public static string GetAbbreviation(string? source)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            return string.Empty;
        }

        var trimedData = source.Trim();
        if (trimedData.Length <= 2)
        {
            return trimedData.ToUpper();
        }

        if (trimedData.Contains(' '))
        {
            var splited = trimedData.Split(' ');
            return $"{splited[0].ToUpper()[0]}{splited[1].ToUpper()[0]}";
        }

        var pascalCase = trimedData;
        pascalCase = trimedData.ToUpper()[0] + pascalCase.Substring(1);
        var upperCaseOnly = string.Concat(pascalCase.Where(char.IsUpper));
        if (upperCaseOnly.Length > 1 && upperCaseOnly.Length <= 3)
        {
            return upperCaseOnly.Substring(0, 2).ToUpper();
        }

        if (trimedData.Length <= 3)
        {
            return trimedData.Substring(0, 2).ToUpper();
        }

        return trimedData.Substring(0, 2).ToUpper();
    }

    /// <summary>Concatène plusieurs chaines et enlève les espaces superflus ou renvoie null si la chaine est vide</summary>
    /// <param name="source">Les chaines a formater</param>
    /// <returns>La chaine formatée </returns>
    public static string? Join(params object?[] source)
    {
        var chainesTemp = source.ToList().Where(s => s != null).Select(s => s!.ToString()!.Trim()).ToList();

        if (!chainesTemp.Any())
        {
            return string.Empty;
        }

        var chaine = string.Join(" ", chainesTemp);

        if (string.IsNullOrWhiteSpace(chaine))
        {
            return null;
        }

        return chaine.Trim();
    }

    public static string? KeepDigitsOnly(string? source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return source;
        }

        return new string(source.Where(char.IsDigit).ToArray());
    }

    public static string RandomString(int length)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
                                    .Select(s => s[random.Next(s.Length)])
                                    .ToArray());
    }

    /// <summary>
    /// Essaye de convertir en int, si echec on retourne 0
    /// </summary>
    /// <param name="source">la chaine de caractère à convertir</param>
    /// <returns>la chaine convertie, zero si echec</returns>
    public static int ToInteger(string? source)
    {
        var result = int.TryParse(source, out var number);
        if (result)
        {
            return number;
        }

        return 0;
    }

    /// <summary>
    /// Essaye de convertir en int, si echec on retourne 0
    /// </summary>
    /// <param name="source">la chaine de caractère à convertir</param>
    /// <returns>la chaine convertie, zero si echec</returns>
    public static long ToLong(string? source)
    {
        var result = long.TryParse(source, out var number);
        if (result)
        {
            return number;
        }

        return 0;
    }

    /// <summary>
    /// Supprime, de l'objet System.String actuel, tous les espaces blancs en début ou en fin de chaîne.
    /// </summary>
    /// <param name="source">la chaine de caractère</param>
    /// <param name="allWhiteSpace">
    /// Si allWhiteSpace est vrai, on supprime aussi tous les espaces blancs à l'intérieur de la
    /// chaine de caractères
    /// </param>
    /// <returns>une chaine de caractères sans espaces blancs</returns>
    public static string Trim(string? source, bool allWhiteSpace = false)
    {
        if (string.IsNullOrEmpty(source))
        {
            return string.Empty;
        }

        if (allWhiteSpace)
        {
            return Regex.Replace(source, @"\s+", string.Empty, RegexOptions.None, RegexHelper.MatchTimeout);
        }

        return source.Trim();
    }

    /// <summary>Enleve les espaces superflus d'une chaine uniquement ou renvoie null si la chaine est vide</summary>
    /// <param name="source">La chaine a formater</param>
    /// <returns>La chaine formatée </returns>
    public static string? Trim(object? source)
    {
        if (source == null)
        {
            return null;
        }

        var value = source.ToString();
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim();
    }

    /// <summary>
    /// Enleve les espaces superflus d'une chaine uniquement ou renvoie string.Empty si la chaine est vide.
    /// </summary>
    /// <param name="source">La chaine à formater.</param>
    /// <returns>La chaine formatée.</returns>
    public static string TrimIfNotNull(object? source)
    {
        if (source == null)
        {
            return string.Empty;
        }

        var value = source.ToString();
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        return value.Trim();
    }

    public static string? Truncate(string? source, int maxLength)
    {
        if (string.IsNullOrEmpty(source))
        {
            return source;
        }

        return source.Length <= maxLength
            ? source
            : source.Substring(0, maxLength) + "...";
    }

    public static bool TryParseToBoolean(string? source)
    {
        var result = bool.TryParse(source, out var boolValue);
        if (result)
        {
            return boolValue;
        }

        return false;
    }
}