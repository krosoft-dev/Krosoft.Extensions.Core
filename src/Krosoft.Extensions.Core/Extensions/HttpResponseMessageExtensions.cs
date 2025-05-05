using System.Net;
using Krosoft.Extensions.Core.Converters;
using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Models.Dto;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Models.Exceptions.Http;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Extensions;

public static class HttpResponseMessageExtensions
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

    public static async Task EnsureAsync(this HttpResponseMessage httpResponseMessage,
                                         CancellationToken cancellationToken = default)
    {
        if (httpResponseMessage.IsSuccessStatusCode)
        {
        }
        else
        {
            var json = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

            var isValid = JsonHelper.IsValid(json);
            if (isValid && httpResponseMessage.StatusCode == HttpStatusCode.BadRequest)
            {
                var obj = JsonConvert.DeserializeObject<KrosoftFunctionalException>(json, new KrosoftFunctionalExceptionConverter());
                if (obj != null)
                {
                    throw obj;
                }
            }

            throw new HttpException(httpResponseMessage.StatusCode,
                                    httpResponseMessage.ReasonPhrase);
        }
    }

    public static async Task<T?> EnsureAsync<T>(this HttpResponseMessage httpResponseMessage,
                                                Func<HttpStatusCode, string, Exception>? onError,
                                                CancellationToken cancellationToken = default)
    {
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var json = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            var obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }

        await httpResponseMessage.EnsureAsync(onError, cancellationToken);
        return default;
    }

    public static Task<T?> EnsureAsync<T>(this HttpResponseMessage httpResponseMessage,
                                          CancellationToken cancellationToken = default) =>
        httpResponseMessage.EnsureAsync<T>(null, cancellationToken);

    public static async Task EnsureAsync(this HttpResponseMessage httpResponseMessage,
                                         Func<HttpStatusCode, string, Exception>? onError,
                                         CancellationToken cancellationToken = default)
    {
        if (httpResponseMessage.IsSuccessStatusCode)
        {
        }
        else
        {
            var json = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            var isValid = JsonHelper.IsValid(json);
            if (isValid)
            {
                var ex = onError?.Invoke(httpResponseMessage.StatusCode, json);
                if (ex != null)
                {
                    throw ex;
                }
            }

            throw new HttpException(httpResponseMessage.StatusCode,
                                    httpResponseMessage.ReasonPhrase);
        }
    }

    public static Task<IFileStream?> EnsureStreamAsync(this Task<HttpResponseMessage> task,
                                                       CancellationToken cancellationToken = default) =>
        task.EnsureStreamAsync(null, cancellationToken);

    public static async Task<IFileStream?> EnsureStreamAsync(this Task<HttpResponseMessage> task,
                                                             Func<HttpStatusCode, string, Exception>? onError,
                                                             CancellationToken cancellationToken = default)
    {
        var httpResponseMessage = await task;

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var contentType = httpResponseMessage.Content.Headers.ContentType?.ToString() ?? string.Empty;
            var contentDisposition = httpResponseMessage.Content.Headers.ContentDisposition?.FileName?.Trim('"') ?? string.Empty;

            var stream = await httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken);

            return new GenericFileStream(stream, contentDisposition, contentType);
        }

        await httpResponseMessage.EnsureAsync(onError, cancellationToken);
        return null;
    }

    public static async Task<string?> EnsureStringAsync(this Task<HttpResponseMessage> task,
                                                        CancellationToken cancellationToken = default)
    {
        var httpResponseMessage = await task;

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var template = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

            return template;
        }

        await httpResponseMessage.ManageErrorAsync(cancellationToken);

        return null;
    }

    public static async Task ManageErrorAsync(this HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken)
    {
        var json = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

        var isValid = JsonHelper.IsValid(json);
        if (isValid)
        {
            var obj = JsonConvert.DeserializeObject<ErrorApiDto>(json);
            if (obj != null)
            {
                if (Enum.TryParse(obj.StatusCode, out HttpStatusCode value) && Enum.IsDefined(typeof(HttpStatusCode), value))
                {
                    throw new HttpException(value, obj.Message);
                }

                throw new HttpException(httpResponseMessage.StatusCode,
                                        obj.Message);
            }
        }

        throw new HttpException(httpResponseMessage.StatusCode,
                                httpResponseMessage.ReasonPhrase);
    }
}