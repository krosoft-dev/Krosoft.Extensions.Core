using System.Security;
using Krosoft.Extensions.Core.Attributes;
using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Core.Tools;

[SecuritySafeCritical]
public static class Guard
{
    [SecuritySafeCritical]
    public static void IsNotNull(string argumentName, [ValidatedNotNull] object? value)
    {
        if (value == null)
        {
            throw new KrosoftTechnicalException($"La variable '{argumentName}' n'est pas renseignée.");
        }
    }

    [SecuritySafeCritical]
    public static void IsNotNullOrWhiteSpace(string argumentName, [ValidatedNotNull] string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new KrosoftTechnicalException($"La variable '{argumentName}' est vide ou non renseignée.");
        }
    }
}