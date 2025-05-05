namespace Krosoft.Extensions.Core.Helpers;

public static class StringComparaisonHelper
{
    /* The Winkler modification will not be applied unless the
     * percent match was at or above the mWeightThreshold percent
     * without the modification.
     * Winkler's paper used a default value of 0.7
     */
    private static readonly decimal WeightThreshold = 0.7m;

    /* Size of the prefix to be concidered by the Winkler modification.
     * Winkler's paper used a default value of 4
     */
    private static readonly int NumChars = 4;

    /// <summary>
    /// Returns the Jaro-Winkler distance between the specified
    /// strings. The distance is symmetric and will fall in the
    /// range 0 (no match) to 1 (perfect match).
    /// </summary>
    /// <param name="source">First String</param>
    /// <param name="target">Second String</param>
    /// <returns></returns>
    public static decimal CalculateJaroWinkler(string source, string target)
    {
        var lLen1 = source.Length;
        var lLen2 = target.Length;
        if (lLen1 == 0)
        {
            return lLen2 == 0 ? 1.0m : 0.0m;
        }

        var lSearchRange = Math.Max(0, Math.Max(lLen1, lLen2) / 2 - 1);

        // default initialized to false
        var lMatched1 = new bool[lLen1];
        var lMatched2 = new bool[lLen2];

        var lNumCommon = 0;
        for (var i = 0; i < lLen1; ++i)
        {
            var lStart = Math.Max(0, i - lSearchRange);
            var lEnd = Math.Min(i + lSearchRange + 1, lLen2);
            for (var j = lStart; j < lEnd; ++j)
            {
                if (lMatched2[j])
                {
                    continue;
                }

                if (source[i] != target[j])
                {
                    continue;
                }

                lMatched1[i] = true;
                lMatched2[j] = true;
                ++lNumCommon;
                break;
            }
        }

        if (lNumCommon == 0)
        {
            return 0.0m;
        }

        var lNumHalfTransposed = 0;
        var k = 0;
        for (var i = 0; i < lLen1; ++i)
        {
            if (!lMatched1[i])
            {
                continue;
            }

            while (!lMatched2[k])
            {
                ++k;
            }

            if (source[i] != target[k])
            {
                ++lNumHalfTransposed;
            }

            ++k;
        }

        var lNumTransposed = lNumHalfTransposed / 2;

        decimal lNumCommonD = lNumCommon;
        var lWeight = (lNumCommonD / lLen1 + lNumCommonD / lLen2 + (lNumCommon - lNumTransposed) / lNumCommonD) / 3.0m;

        if (lWeight <= WeightThreshold)
        {
            return lWeight;
        }

        var lMax = Math.Min(NumChars, Math.Min(source.Length, target.Length));
        var lPos = 0;
        while (lPos < lMax && source[lPos] == target[lPos])
        {
            ++lPos;
        }

        if (lPos == 0)
        {
            return lWeight;
        }

        return lWeight + 0.1m * lPos * (1.0m - lWeight);
    }

    public static decimal CalculateJaroWinklerInPourcent(string source, string target) => CalculateJaroWinkler(source, target) * 100;

    public static int CalculateLevenshtein(string source, string target)
    {
        if (string.IsNullOrEmpty(source))
        {
            return string.IsNullOrEmpty(target) ? 0 : target.Length;
        }

        if (string.IsNullOrEmpty(target))
        {
            return string.IsNullOrEmpty(source) ? 0 : source.Length;
        }

        var sourceLength = source.Length;
        var targetLength = target.Length;

        var distance = new int[sourceLength + 1, targetLength + 1];

        // Step 1
        for (var i = 0; i <= sourceLength; distance[i, 0] = i++)
        {
        }

        for (var j = 0; j <= targetLength; distance[0, j] = j++)
        {
        }

        for (var i = 1; i <= sourceLength; i++)
        {
            for (var j = 1; j <= targetLength; j++)
            {
                // Step 2
                var cost = target[j - 1] == source[i - 1] ? 0 : 1;

                // Step 3
                distance[i, j] = Math.Min(
                                          Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
                                          distance[i - 1, j - 1] + cost);
            }
        }

        return distance[sourceLength, targetLength];
    }

    /// <summary>
    /// Calculate percentage similarity of two strings
    /// <param name="source">Source String to Compare with</param>
    /// <param name="target">Targeted String to Compare</param>
    /// <returns>Return Similarity between two strings from 0 to 1.0</returns>
    /// </summary>
    public static decimal CalculateLevenshteinInPourcent(string source, string target)
    {
        if (string.IsNullOrEmpty(source))
        {
            return string.IsNullOrEmpty(target) ? 1 : 0;
        }

        if (string.IsNullOrEmpty(target))
        {
            return string.IsNullOrEmpty(source) ? 1 : 0;
        }

        var stepsToSame = CalculateLevenshtein(source, target);
        return (1.0m - stepsToSame / (decimal)Math.Max(source.Length, target.Length)) * 100;
    }
}