namespace Krosoft.Extensions.Core.Extensions;

public static class DecimalExtension
{
    public static bool IsBetween(this decimal? value, decimal borneInf, decimal borneSup) => value.HasValue && value.Value.IsBetween(borneInf, borneSup, false);

    public static bool IsBetween(this decimal value, decimal borneInf, decimal borneSup) => value.IsBetween(borneInf, borneSup, false);

    public static bool IsBetween(this decimal? value, decimal borneInf, decimal borneSup, bool isStrict) => value.HasValue && value.Value.IsBetween(borneInf, borneSup, isStrict);

    public static bool IsBetween(this decimal value, decimal borneInf, decimal borneSup, bool isStrict)
    {
        if (isStrict)
        {
            return value > borneInf && value < borneSup;
        }

        return value >= borneInf && value <= borneSup;
    }
}