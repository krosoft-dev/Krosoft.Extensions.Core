using Krosoft.Extensions.Core.Tools;

namespace Krosoft.Extensions.Core.Extensions;

/// <summary>
/// Extensions methods for Task class
/// </summary>
public static class TaskExtensions
{
    public static async Task<IEnumerable<TSource>?> DistinctBy<TKey, TSource>(this Task<IEnumerable<TSource>?> task,
                                                                              Func<TSource, TKey> func)
    {
        Guard.IsNotNull(nameof(task), task);
        var items = await task;
        if (items != null)
        {
            var itemsPar = items.DistinctBy(func);
            return itemsPar;
        }

        return new List<TSource>();
    }

    public static async Task<TSource?> FirstOrDefault<TSource>(this Task<IEnumerable<TSource>?> task, Func<TSource, bool> predicate)
    {
        Guard.IsNotNull(nameof(task), task);

        var items = await task;
        if (items != null)
        {
            return items.FirstOrDefault(predicate);
        }

        return default;
    }

    public static async Task<TSource?> FirstOrDefault<TSource>(this Task<IEnumerable<TSource>?> task)
    {
        Guard.IsNotNull(nameof(task), task);

        var items = await task;
        if (items != null)
        {
            return items.FirstOrDefault();
        }

        return default;
    }

    public static async Task<IEnumerable<IGrouping<TKey, TSource>>?> GroupBy<TSource, TKey>(this Task<IEnumerable<TSource>?> task, Func<TSource, TKey> keySelector)
    {
        Guard.IsNotNull(nameof(task), task);

        var items = await task;
        if (items != null)
        {
            return items.GroupBy(keySelector);
        }

        return default;
    }

    /// <summary>
    /// Throws a TimeoutException if a task is not completed before a delay
    /// </summary>
    /// <typeparam name="T">return type</typeparam>
    /// <param name="task">task</param>
    /// <param name="delay">the delay in milliseconds</param>
    /// <returns>T</returns>
    public static async Task<T> TimeoutAfter<T>(this Task<T> task, int delay)
    {
        await Task.WhenAny(task, Task.Delay(delay));
        if (!task.IsCompleted)
        {
            throw new TimeoutException();
        }

        return await task;
    }

    public static async Task<IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this Task<IEnumerable<TSource>?> task,
                                                                                                Func<TSource, TKey> keySelector,
                                                                                                Func<TSource, TElement> elementSelector,
                                                                                                bool useDistinct) where TKey : notnull
    {
        Guard.IsNotNull(nameof(task), task);
        var items = await task;
        if (items != null)
        {
            var itemsPar = items.ToDictionary(keySelector, elementSelector, useDistinct);
            return itemsPar;
        }

        return new Dictionary<TKey, TElement>();
    }

    public static async Task<Dictionary<TKey, TSource>> ToDictionary<TKey, TSource>(this Task<IEnumerable<TSource>?> task,
                                                                                    Func<TSource, TKey> func) where TKey : notnull
    {
        Guard.IsNotNull(nameof(task), task);
        var items = await task;
        if (items != null)
        {
            var itemsPar = items.ToDictionary(func);
            return itemsPar;
        }

        return new Dictionary<TKey, TSource>();
    }

    public static async Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this Task<IEnumerable<TSource>?> task,
                                                                                               Func<TSource, TKey> keySelector,
                                                                                               Func<TSource, TElement> elementSelector) where TKey : notnull
    {
        Guard.IsNotNull(nameof(task), task);
        var items = await task;
        if (items != null)
        {
            var itemsPar = items.ToDictionary(keySelector, elementSelector);
            return itemsPar;
        }

        return new Dictionary<TKey, TElement>();
    }

    public static async Task<IDictionary<TKey, TSource>> ToDictionary<TKey, TSource>(this Task<IEnumerable<TSource>?> task,
                                                                                     Func<TSource, TKey> func,
                                                                                     bool useDistinct) where TKey : notnull
    {
        Guard.IsNotNull(nameof(task), task);
        var items = await task;
        if (items != null)
        {
            var itemsPar = items.ToDictionary(func, useDistinct);
            return itemsPar;
        }

        return new Dictionary<TKey, TSource>();
    }

    public static async Task<HashSet<TSource>> ToHashSet<TSource>(this Task<IEnumerable<TSource>?> task)
    {
        Guard.IsNotNull(nameof(task), task);

        var items = await task;
        if (items != null)
        {
            return items.ToHashSet();
        }

        return new HashSet<TSource>();
    }

    public static async Task<List<TSource>> ToList<TSource>(this Task<IEnumerable<TSource>?> task)
    {
        Guard.IsNotNull(nameof(task), task);

        var items = await task;
        if (items != null)
        {
            return items.ToList();
        }

        return new List<TSource>();
    }

    public static async Task<ILookup<TKey, TSource>> ToLookup<TKey, TSource>(this Task<IEnumerable<TSource>?> task,
                                                                             Func<TSource, TKey> func)
    {
        Guard.IsNotNull(nameof(task), task);
        var items = await task;
        if (items != null)
        {
            var itemsPar = items.ToLookup(func);
            return itemsPar;
        }

        return new List<TSource>().ToLookup(func);
    }

    public static async Task<IReadOnlyDictionary<TKey, TSource>> ToReadOnlyDictionary<TKey, TSource>(this Task<IEnumerable<TSource>?> task,
                                                                                                     Func<TSource, TKey> func,
                                                                                                     bool useDistinct) where TKey : notnull
    {
        Guard.IsNotNull(nameof(task), task);
        var items = await task;
        if (items != null)
        {
            var itemsPar = items.ToReadOnlyDictionary(func, useDistinct);
            return itemsPar;
        }

        return new Dictionary<TKey, TSource>();
    }
}