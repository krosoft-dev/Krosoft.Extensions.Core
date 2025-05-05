namespace Krosoft.Extensions.Core.Helpers;

public static class ActionHelper
{
    public static async Task<IEnumerable<TSource>> ApplyWithAsync<TKey, TSource>(IEnumerable<TKey>? keys,
                                                                                 Func<ISet<TKey>, Task<IEnumerable<TSource>>> func)
    {
        if (keys != null)
        {
            var uniqueKeys = keys.ToHashSet();
            if (uniqueKeys.Any())
            {
                var items = await func(uniqueKeys);

                return items;
            }
        }

        return new List<TSource>();
    }
}