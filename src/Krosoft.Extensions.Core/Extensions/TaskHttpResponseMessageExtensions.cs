using System.Net;
using Krosoft.Extensions.Core.Models;

namespace Krosoft.Extensions.Core.Extensions;

public static class TaskHttpResponseMessageExtensions
{
    public static Task EnsureAsync(this Task<HttpResponseMessage> task,
                                   CancellationToken cancellationToken = default)
        => task.EnsureAsync(null, cancellationToken);

    public static async Task EnsureAsync(this Task<HttpResponseMessage> task,
                                         Func<HttpStatusCode, string, Exception>? onError,
                                         CancellationToken cancellationToken = default)
    {
        var httpResponseMessage = await task;
        await httpResponseMessage.EnsureAsync(onError, cancellationToken);
    }

    public static Task<T?> EnsureAsync<T>(this Task<HttpResponseMessage> task,
                                          CancellationToken cancellationToken = default) =>
        task.EnsureAsync<T?>(null, cancellationToken);

    public static async Task<T?> EnsureAsync<T>(this Task<HttpResponseMessage> task,
                                                Func<HttpStatusCode, string, Exception>? onError,
                                                CancellationToken cancellationToken = default)
    {
        var httpResponseMessage = await task;
        return await httpResponseMessage.EnsureAsync<T?>(onError, cancellationToken);
    }

    public static async Task<string?> EnsureStringAsync(this Task<HttpResponseMessage> task,
                                                        CancellationToken cancellationToken = default)

    {
        var httpResponseMessage = await task;
        return await httpResponseMessage.EnsureStringAsync(cancellationToken);
    }

    public static Task<IFileStream?> EnsureStreamAsync(this Task<HttpResponseMessage> task,
                                                       CancellationToken cancellationToken = default) =>
        task.EnsureStreamAsync(null, cancellationToken);

    public static async Task<IFileStream?> EnsureStreamAsync(this Task<HttpResponseMessage> task,
                                                             Func<HttpStatusCode, string, Exception>? onError,
                                                             CancellationToken cancellationToken = default)
    {
        var httpResponseMessage = await task;
        return await httpResponseMessage.EnsureStreamAsync(onError, cancellationToken);
    }

    public static Task<Result<T?>> EnsureResultAsync<T>(this Task<HttpResponseMessage> task,
                                                        CancellationToken cancellationToken = default) =>
        task.EnsureResultAsync<T>(null, cancellationToken);

    public static Task EnsureResultAsync(this Task<HttpResponseMessage> task,
                                         CancellationToken cancellationToken = default)
        => task.EnsureAsync(null, cancellationToken);

    public static async Task<Result<T?>> EnsureResultAsync<T>(this Task<HttpResponseMessage> task,
                                                              Func<HttpStatusCode, string, Exception>? onError,
                                                              CancellationToken cancellationToken = default)
    {
        var httpResponseMessage = await task;
        return await httpResponseMessage.EnsureResultAsync<T?>(onError, cancellationToken);
    }

    public static async Task<T?> EnsureXmlAsync<T>(this Task<HttpResponseMessage> task,
                                                   CancellationToken cancellationToken = default)
    {
        var httpResponseMessage = await task;
        return await httpResponseMessage.EnsureXmlAsync<T>(cancellationToken);
    }

    public static async Task<Result<T?>> EnsureResultXmlAsync<T>(this Task<HttpResponseMessage> task,
                                                                 CancellationToken cancellationToken = default)
    {
        var httpResponseMessage = await task;
        return await httpResponseMessage.EnsureResultXmlAsync<T>(cancellationToken);
    }

    public static async Task<Result<string?>> EnsureResultStringAsync(this Task<HttpResponseMessage> task,
                                                                      CancellationToken cancellationToken = default)
    {
        var httpResponseMessage = await task;
        return await httpResponseMessage.EnsureResultStringAsync(cancellationToken);
    }

    public static async Task<Result<IFileStream?>> EnsureResultStreamAsync(this Task<HttpResponseMessage> task,
                                                                           CancellationToken cancellationToken = default)
    {
        var httpResponseMessage = await task;
        return await httpResponseMessage.EnsureResultStreamAsync(cancellationToken);
    }
}