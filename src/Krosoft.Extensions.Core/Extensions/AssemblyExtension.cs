using System.Reflection;
using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Core.Tools;

namespace Krosoft.Extensions.Core.Extensions;

/// <summary>
/// Méthodes d'extensions pour la classe <see cref="Assembly" />.
/// </summary>
public static class AssemblyExtension
{
    public static Stream Read(this Assembly assembly, string resourceName)
    {
        Guard.IsNotNull(nameof(assembly), assembly);
        Guard.IsNotNullOrWhiteSpace(nameof(resourceName), resourceName);

        return AssemblyHelper.Read(assembly, resourceName);
    }
}