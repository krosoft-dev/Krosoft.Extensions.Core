using System.Text.RegularExpressions;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Tools;

namespace Krosoft.Extensions.Core.Helpers;

public static class ImageHelper
{
    public static bool CheckImage(string? base64, string? name)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(base64), base64);
        Guard.IsNotNullOrWhiteSpace(nameof(name), name);

        var match = Regex.Match(base64!, @"data:(?<type>.+?);base64,(?<data>.+)"
                                , RegexOptions.None, RegexHelper.MatchTimeout);
        var base64Data = match.Groups["data"].Value;
        if (string.IsNullOrEmpty(base64Data))
        {
            throw new KrosoftTechnicalException($"Impossible de récupérer l'image à partir de l'url Base64 pour l'image '{name!}'.");
        }

        return true;
    }
}