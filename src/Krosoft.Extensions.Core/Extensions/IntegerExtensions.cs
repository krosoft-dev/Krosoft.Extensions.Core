namespace Krosoft.Extensions.Core.Extensions;

public static class IntegerExtensions
{
    public static bool IsBetween(this int num,
                                 int lower,
                                 int upper,
                                 bool inclusive = true) =>
        inclusive
            ? lower <= num && num <= upper
            : lower < num && num < upper;

    /// <summary>
    /// Méthode d'extension pour transformer un entier en Guid.
    /// </summary>
    /// <param name="i">Entier à transformer</param>
    /// <returns>Un Guid valable</returns>
    public static Guid ToGuid(this long i) => new Guid($"00000000-0000-0000-0000-{i:D12}");

    /// <summary>
    /// Méthode d'extension pour transformer un entier en Guid.
    /// </summary>
    /// <param name="i">Entier à transformer</param>
    /// <returns>Un Guid valable</returns>
    public static Guid ToGuid(this int i) => new Guid($"00000000-0000-0000-0000-{i:D12}");
}