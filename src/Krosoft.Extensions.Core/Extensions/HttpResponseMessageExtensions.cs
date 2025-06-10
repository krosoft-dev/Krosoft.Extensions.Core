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
            if (isValid)
            {
                var exception = TryThrowFromJson(httpResponseMessage.StatusCode, json);
                if (exception != null)
                {
                    throw exception;
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

                var exception = TryThrowFromJson(httpResponseMessage.StatusCode, json);
                if (exception != null)
                {
                    throw exception;
                }
            }

            throw new HttpException(httpResponseMessage.StatusCode,
                                    httpResponseMessage.ReasonPhrase);
        }
    }

    public static async Task<T?> EnsureXmlAsync<T>(this HttpResponseMessage httpResponseMessage,
                                                   CancellationToken cancellationToken = default)
    {
        var response = await httpResponseMessage.EnsureStringAsync(cancellationToken);
        return XmlHelper.Deserialize<T>(response);
    }

    public static async Task<Result<T?>> EnsureResultXmlAsync<T>(this HttpResponseMessage httpResponseMessage,
                                                                 CancellationToken cancellationToken = default)
    {
        var response = await httpResponseMessage.EnsureResultStringAsync(cancellationToken);
        if (response.IsFaulted)
        {
            return Result<T?>.Failure(response.Exception!);
        }

        return Result<T?>.Success(XmlHelper.Deserialize<T>(response.Value));
    }

    public static async Task<string?> EnsureStringAsync(this HttpResponseMessage httpResponseMessage,
                                                        CancellationToken cancellationToken = default)
    {
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var value = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            return value;
        }

        await httpResponseMessage.ManageErrorAsync(cancellationToken);

        return null;
    }

    public static async Task<Result<string?>> EnsureResultStringAsync(this HttpResponseMessage httpResponseMessage,
                                                                      CancellationToken cancellationToken = default)
    {
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

            return Result<string?>.Success(content);
        }

        return await httpResponseMessage.ManageResultErrorAsync<string?>(cancellationToken);
    }

    public static async Task<Result<IFileStream?>> EnsureResultStreamAsync(this HttpResponseMessage httpResponseMessage,
                                                                           CancellationToken cancellationToken = default)
    {
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var contentType = httpResponseMessage.Content.Headers.ContentType?.ToString() ?? string.Empty;
            var contentDisposition = httpResponseMessage.Content.Headers.ContentDisposition?.FileName?.Trim('"') ?? string.Empty;

            var stream = await httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken);

            return Result<IFileStream?>.Success(new GenericFileStream(stream, contentDisposition, contentType));
        }

        return await httpResponseMessage.ManageResultErrorAsync<IFileStream?>(cancellationToken);
    }

    public static async Task EnsureResultAsync(this HttpResponseMessage httpResponseMessage,
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
                var exception = TryThrowFromJson(httpResponseMessage.StatusCode, json);
                if (exception != null)
                {
                    throw exception;
                }
            }

            throw new HttpException(httpResponseMessage.StatusCode,
                                    httpResponseMessage.ReasonPhrase);
        }
    }

    public static Task<Result<T?>> EnsureResultAsync<T>(this HttpResponseMessage httpResponseMessage,
                                                        CancellationToken cancellationToken = default) =>
        httpResponseMessage.EnsureResultAsync<T>(null, cancellationToken);

    public static async Task<IFileStream?> EnsureStreamAsync(this HttpResponseMessage httpResponseMessage,
                                                             Func<HttpStatusCode, string, Exception>? onError,
                                                             CancellationToken cancellationToken = default)
    {
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

    public static async Task<Result<T?>> EnsureResultAsync<T>(this HttpResponseMessage httpResponseMessage,
                                                              Func<HttpStatusCode, string, Exception>? onError,
                                                              CancellationToken cancellationToken = default)
    {
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var json = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            var obj = JsonConvert.DeserializeObject<T>(json);
            return Result<T?>.Success(obj);
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
                    return Result<T?>.Failure(ex);
                }

                var exception = TryThrowFromJson(httpResponseMessage.StatusCode, json);
                if (exception != null)
                {
                    return Result<T?>.Failure(exception);
                }
            }

            return Result<T?>.Failure(new HttpException(httpResponseMessage.StatusCode,
                                                        httpResponseMessage.ReasonPhrase));
        }
    }

    public static async Task ManageErrorAsync(this HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken)
    {
        var json = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

        var isValid = JsonHelper.IsValid(json);
        if (isValid)
        {
            var exception = TryThrowFromJson(httpResponseMessage.StatusCode, json);
            if (exception != null)
            {
                throw exception;
            }
        }

        throw new HttpException(httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase);
    }

    public static async Task<Result<T?>> ManageResultErrorAsync<T>(this HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken)
    {
        var json = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

        var isValid = JsonHelper.IsValid(json);
        if (isValid)
        {
            var exception = TryThrowFromJson(httpResponseMessage.StatusCode, json);
            if (exception != null)
            {
                throw exception;
            }
        }

        throw new HttpException(httpResponseMessage.StatusCode,
                                httpResponseMessage.ReasonPhrase);
    }

    private static Exception? TryThrowFromJson(HttpStatusCode statusCode, string json)
    {
        if (statusCode == HttpStatusCode.BadRequest)
        {
            var obj = JsonConvert.DeserializeObject<KrosoftFunctionalException>(json, new KrosoftFunctionalExceptionConverter());
            if (obj != null)
            {
                throw obj;
            }
        }

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            var obj = JsonConvert.DeserializeObject<KrosoftTechnicalException>(json, new KrosoftTechnicalExceptionConverter());
            if (obj != null)
            {
                throw obj;
            }
        }

        var errorDto = JsonConvert.DeserializeObject<ErrorDto>(json);
        if (errorDto != null && errorDto.Code != 0 && !string.IsNullOrEmpty(errorDto.Message))
        {
            if (Enum.TryParse(errorDto.Code.ToString(), out HttpStatusCode value) && Enum.IsDefined(typeof(HttpStatusCode), value))
            {
                throw new HttpException(value, errorDto.Message);
            }

            throw new HttpException(statusCode, errorDto.Message);
        }

        return null;
    }
}