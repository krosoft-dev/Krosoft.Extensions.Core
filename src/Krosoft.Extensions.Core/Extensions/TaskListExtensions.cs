using Krosoft.Extensions.Core.Tools;

namespace Krosoft.Extensions.Core.Extensions;

public static class TaskListExtensions
{
    public static async Task<IEnumerable<TSource>> AsEnumerable<TSource>(this Task<List<TSource>?> task)
    {
        Guard.IsNotNull(nameof(task), task);

        var items = await task;
        if (items != null)
        {
            var itemsPar = items.AsEnumerable();
            return itemsPar;
        }

        return new List<TSource>();
    }

    public static Task<IEnumerable<TSource>> DistinctBy<TKey, TSource>(this Task<List<TSource>> task,
                                                                       Func<TSource, TKey> func)
    {
        Guard.IsNotNull(nameof(task), task);

        return task!.AsEnumerable()!.DistinctBy(func)!;
    }

    public static Task<TSource?> FirstOrDefault<TSource>(this Task<List<TSource?>?>? task)
    {
        Guard.IsNotNull(nameof(task), task);

        return task!.AsEnumerable()!.FirstOrDefault();
    }

    public static Task<TSource?> FirstOrDefault<TSource>(this Task<List<TSource>> task, Func<TSource, bool> predicate)
    {
        Guard.IsNotNull(nameof(task), task);

        return task!.AsEnumerable()!.FirstOrDefault(predicate);
    }

    public static Task<IEnumerable<IGrouping<TKey, TSource>>?> GroupBy<TKey, TSource>(this Task<List<TSource>> task, Func<TSource, TKey> keySelector)
    {
        Guard.IsNotNull(nameof(task), task);

        return task!.AsEnumerable()!.GroupBy(keySelector);
    }

    public static Task<Dictionary<TKey, TSource>> ToDictionary<TKey, TSource>(this Task<List<TSource>?> task,
                                                                              Func<TSource, TKey> func) where TKey : notnull
    {
        Guard.IsNotNull(nameof(task), task);

        return task.AsEnumerable()!.ToDictionary(func);
    }

    public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this Task<List<TSource>?> task,
                                                                                         Func<TSource, TKey> keySelector,
                                                                                         Func<TSource, TElement> elementSelector) where TKey : notnull
    {
        Guard.IsNotNull(nameof(task), task);

        return task.AsEnumerable()!.ToDictionary(keySelector, elementSelector);
    }

    public static Task<IDictionary<TKey, TSource>> ToDictionary<TKey, TSource>(this Task<List<TSource>?> task,
                                                                               Func<TSource, TKey> func,
                                                                               bool useDistinct) where TKey : notnull
    {
        Guard.IsNotNull(nameof(task), task);

        return task.AsEnumerable()!.ToDictionary(func, useDistinct);
    }

    public static Task<HashSet<TSource>> ToHashSet<TSource>(this Task<List<TSource>> task)
    {
        Guard.IsNotNull(nameof(task), task);

        return task!.AsEnumerable()!.ToHashSet();
    }

    public static Task<ILookup<TKey, TSource>> ToLookup<TKey, TSource>(this Task<List<TSource>?> task,
                                                                       Func<TSource, TKey> func)
    {
        Guard.IsNotNull(nameof(task), task);

        return task.AsEnumerable()!.ToLookup(func);
    }

    public static Task<IReadOnlyDictionary<TKey, TSource>> ToReadOnlyDictionary<TKey, TSource>(this Task<List<TSource>?> task,
                                                                                               Func<TSource, TKey> func,
                                                                                               bool useDistinct) where TKey : notnull
    {
        Guard.IsNotNull(nameof(task), task);

        return task.AsEnumerable()!.ToReadOnlyDictionary(func, useDistinct);
    }
}