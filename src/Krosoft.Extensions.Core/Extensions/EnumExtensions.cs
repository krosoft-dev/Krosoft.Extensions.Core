using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Krosoft.Extensions.Core.Tools;

namespace Krosoft.Extensions.Core.Extensions;

/// <summary>
/// Méthodes d'extension pour les énumérations.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Retourne la description à afficher d'une valeur d'énumération, soit depuis l'attribut
    /// <see cref="DescriptionAttribute" />,
    /// soit depuis la valeur si l'attribut n'est pas présent.
    /// </summary>
    /// <param name="value">Valeur d'énumération pour laquelle retourner le nom.</param>
    /// <returns>Nom à afficher de la valeur d'énumération.</returns>
    public static string GetDescription(this Enum? value)
    {
        Guard.IsNotNull(nameof(value), value);

        var type = value!.GetType();
        var fieldInfo = type.GetField(value.ToString());
        if (fieldInfo != null)
        {
            var attributes = (DescriptionAttribute[])fieldInfo
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
        }

        return value.ToString();
    }

    /// <summary>
    /// Retourne le nom à afficher d'une valeur d'énumération, soit depuis l'attribut <see cref="DisplayNameAttribute" />,
    /// soit depuis la valeur si l'attribut n'est pas présent.
    /// </summary>
    /// <param name="value">Valeur d'énumération pour laquelle retourner le nom.</param>
    /// <returns>Nom à afficher de la valeur d'énumération.</returns>
    public static string GetDisplayName(this Enum? value)
    {
        Guard.IsNotNull(nameof(value), value);

        var type = value!.GetType();
        var fieldInfo = type
            .GetField(value.ToString());
        if (fieldInfo != null)
        {
            var attributes = (DisplayAttribute[])fieldInfo
                .GetCustomAttributes(typeof(DisplayAttribute), false);

            if (attributes.Length > 0)
            {
                var displayName = attributes[0].Name;
                if (displayName != null)
                {
                    return displayName;
                }
            }
        }

        return value.ToString();
    }

    /// <summary>
    /// Checks to see if an enumerated value contains a type.
    /// </summary>
    public static bool Has<T>(this Enum? type, T value)
    {
        try
        {
            return ((int?)(object?)type & (int?)(object?)value) == (int?)(object?)value;
        }
        catch
        {
            return false;
        }
    }
}