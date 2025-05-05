using System.Text;
using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Tools;

namespace Krosoft.Extensions.Core.Extensions;

/// <summary>
/// Méthodes d'extensions pour la classe <see cref="Stream" />.
/// </summary>
public static class StreamExtensions
{
    private static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);

    public static BinaryReader CreateReader(this Stream stream) => new BinaryReader(stream, DefaultEncoding, true);

    public static BinaryWriter CreateWriter(this Stream stream) => new BinaryWriter(stream, DefaultEncoding, true);

    public static byte[] ReadAsByteArray(this Stream stream)
    {
        Guard.IsNotNull(nameof(stream), stream);
        return StreamHelper.ReadAsByteArray(stream);
    }

    public static DateTimeOffset ReadDateTimeOffset(this BinaryReader reader) => new DateTimeOffset(reader.ReadInt64(), TimeSpan.Zero);

    public static T To<T>(this Stream stream)
    {
        Guard.IsNotNull(nameof(stream), stream);

        if (typeof(T) == typeof(Stream))
        {
            return (T)(object)stream;
        }

        if (typeof(T) == typeof(byte[]))
        {
            using (var reader = new BinaryReader(stream))
            {
                return (T)(object)reader.ReadAllBytes();
            }
        }

        if (typeof(T) == typeof(string))
        {
            using (var reader = new StreamReader(stream))
            {
                // Read the content.
                var content = reader.ReadToEnd();
                // Display the content.
                return (T)(object)content;
            }
        }

        throw new KrosoftTechnicalException($"Le type {typeof(T)} n'est pas géré.");
    }

    public static string ToBase64(this Stream stream)
    {
        byte[] bytes;
        if (stream is MemoryStream memoryStream)
        {
            bytes = memoryStream.ToArray();
        }
        else
        {
            bytes = new byte[(int)stream.Length];

            stream.Seek(0, SeekOrigin.Begin);
            var unused = stream.Read(bytes, 0, (int)stream.Length);
        }

        return Convert.ToBase64String(bytes);
    }

    public static byte[] ToByte(this Stream input)
    {
        using (var ms = new MemoryStream())
        {
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }

    public static void Write(this BinaryWriter writer, DateTimeOffset value)
    {
        writer.Write(value.Ticks);
    }

    /// <summary>
    /// Ecrit un stream dans un fichier.
    /// </summary>
    /// <param name="filePath">Chemin du fichier.</param>
    /// <param name="stream">Contenu du fichier.</param>
    public static void Write(this Stream stream, string filePath)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(filePath), filePath);
        Guard.IsNotNull(nameof(stream), stream);

        FileHelper.Write(filePath, stream);
    }

    /// <summary>
    /// Ecrit un stream dans un fichier.
    /// </summary>
    /// <param name="filePath">Chemin du fichier.</param>
    /// <param name="stream">Contenu du fichier.</param>
    /// <param name="cancellationToken">Token d’annulation</param>
    /// <returns>Tache asynchrone.</returns>
    public static async Task WriteAsync(this Stream stream,
                                        string filePath,
                                        CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(filePath), filePath);
        Guard.IsNotNull(nameof(stream), stream);

        await FileHelper.WriteAsync(filePath, stream, cancellationToken)
                        .ConfigureAwait(false);
    }
}