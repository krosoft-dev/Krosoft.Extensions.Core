using System.Security;
using System.Security.Cryptography;
using System.Text;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Tools;

namespace Krosoft.Extensions.Core.Helpers;

public static class FileHelper
{
    private const int BufferSize = 4096;

    public static void CreateDirectoryFromFile(string filePath)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(filePath), filePath);

        var directoryPath = Path.GetDirectoryName(filePath);
        if (string.IsNullOrEmpty(directoryPath))
        {
            throw new KrosoftTechnicalException($"Le chemin {filePath} ne permet pas de déterminer le nom du répertoire.");
        }

        var exists = Directory.Exists(directoryPath);
        if (!exists)
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    public static void CreateFile(string filePath, Stream data)
    {
        using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        data.CopyTo(fileStream);
    }

    public static void CreateFile(string filePath, byte[] data)
    {
        using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        fileStream.Write(data, 0, data.Length);
    }

    public static void DeleteSafely(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    private static IEnumerable<string> GetFilesFromDirectoryRecursively(string sDir)
    {
        var files = new List<string>();

        var directories = Directory.GetDirectories(sDir);
        if (directories.Any())
        {
            foreach (var d in directories)
            {
                foreach (var f in Directory.GetFiles(d))
                {
                    files.Add(f);
                }

                var subFiles = GetFilesFromDirectoryRecursively(d);

                files.AddRange(subFiles);
            }
        }

        return files;
    }

    public static List<string> GetFilesRecursively(string directoryPath)
    {
        var files = new List<string>();

        foreach (var f in Directory.GetFiles(directoryPath))
        {
            files.Add(f);
        }

        var filesFromDirectory = GetFilesFromDirectoryRecursively(directoryPath);

        files.AddRange(filesFromDirectory);

        return files;
    }

    /// <summary>
    /// Permet de savoir si un fichier est déjà en cours d'utilisation.
    /// </summary>
    /// <param name="filePath">Chemin du fichier.</param>
    /// <returns>Vrai si le fichier est utilisé, Faux sinon.</returns>
    public static bool IsFileLocked(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return false;
        }

        try
        {
            var fileTest = File.Open(filePath, FileMode.Open);
            fileTest.Dispose();
            return false;
        }
        catch (IOException)
        {
            return true;
        }
    }

    public static string ReadAsBase64(string filePath)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(filePath), filePath);

        var bytes = File.ReadAllBytes(filePath);
        var encodedData = Convert.ToBase64String(bytes);
        return encodedData;
    }

    public static async Task<string> ReadAsBase64Async(string filePath, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(filePath), filePath);

        var bytes = await File.ReadAllBytesAsync(filePath, cancellationToken);
        var encodedData = Convert.ToBase64String(bytes);
        return encodedData;
    }

    public static string ReadAsHash(string filePath)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(filePath), filePath);

        using (var sha1 = SHA1.Create())
        {
            using (var inputStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var computeHash = sha1.ComputeHash(inputStream);

                var fileHash = BitConverter.ToString(computeHash);

                return fileHash;
            }
        }
    }

    public static string ReadAsMd5Hash(string filePath)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(filePath), filePath);

        using (var md5 = MD5.Create())
        {
            using (var stream = File.OpenRead(filePath))
            {
                return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
            }
        }
    }

    public static IEnumerable<string> ReadAsStringArray(string filePath) => ReadAsStringArray(filePath, EncodingHelper.GetEuropeOccidentale());

    public static IEnumerable<string> ReadAsStringArray(string filePath,
                                                        Encoding encoding)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(filePath), filePath);

        using (var fileStream = File.OpenRead(filePath))
        {
            using (var streamReader = new StreamReader(fileStream, encoding, true))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    if (line != null)
                    {
                        yield return line;
                    }
                }
            }
        }
    }

    public static Task<IEnumerable<string>> ReadAsStringArrayAsync(string filePath) => ReadAsStringArrayAsync(filePath, EncodingHelper.GetEuropeOccidentale());

    public static async Task<IEnumerable<string>> ReadAsStringArrayAsync(string filePath,
                                                                         Encoding encoding)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(filePath), filePath);

        var collection = new List<string>();
        using (var fileStream = File.OpenRead(filePath))
        {
            using (var streamReader = new StreamReader(fileStream, encoding, true))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = await streamReader.ReadLineAsync();
                    if (line != null)
                    {
                        collection.Add(line);
                    }
                }
            }
        }

        return collection;
    }

    public static async Task<string> ReadAsStringAsync(string filePath, CancellationToken cancellationToken)
        => await ReadAsStringAsync(filePath, EncodingHelper.GetEuropeOccidentale(), cancellationToken);

    public static async Task<string> ReadAsStringAsync(string filePath,
                                                       Encoding encoding,
                                                       CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(filePath), filePath);
        Guard.IsNotNull(nameof(encoding), encoding);

        using var sourceReader = new StreamReader(filePath, encoding);

#if NET7_0_OR_GREATER
        return await sourceReader.ReadToEndAsync(cancellationToken);
#else
        return await sourceReader.ReadToEndAsync();
#endif
    }

    public static async Task<byte[]> ReadAsBytesAsync(string filePath,
                                                      CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(filePath), filePath);

        await using var stream = new FileStream(filePath, FileMode.Open);
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms, cancellationToken);
        return ms.ToArray();
    }

    public static byte[] ReadAsBytes(string filePath)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(filePath), filePath);

        using var stream = new FileStream(filePath, FileMode.Open);
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }

    /// <summary>
    /// Ecrit un stream dans un fichier.
    /// </summary>
    /// <param name="filePath">Chemin du fichier.</param>
    /// <param name="stream">Contenu du fichier.</param>
    public static void Write(string filePath,
                             Stream stream)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(filePath), filePath);
        Guard.IsNotNull(nameof(stream), stream);

        var fi = new FileInfo(filePath);
        if (fi.DirectoryName != null)
        {
            DirectoryHelper.CreateDirectoryIfNotExist(fi.DirectoryName);
        }

        using (var fileStream = File.Create(filePath))
        {
            stream.CopyTo(fileStream);
        }
    }

    /// <summary>
    /// Ecrit l'ensemble d'une chaine de caractères dans un fichier.
    /// </summary>
    /// <param name="filePath">Chemin du fichier.</param>
    /// <param name="content">Contenu du fichier.</param>
    public static void Write(string filePath,
                             string content)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(filePath), filePath);
        Guard.IsNotNullOrWhiteSpace(nameof(content), content);

        using var writer = File.CreateText(filePath);
        writer.Write(content);
    }

    public static void WriteAsBase64(string filePath,
                                     string txtEncoded)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(filePath), filePath);

        var bytes = Convert.FromBase64String(txtEncoded);
        File.WriteAllBytes(filePath, bytes);
    }

    /// <summary>
    /// Ecrit l'ensemble d'une chaine de caractères dans un fichier.
    /// </summary>
    /// <param name="filePath">Chemin du fichier.</param>
    /// <param name="content">Contenu du fichier.</param>
    public static async Task WriteAsync(string filePath,
                                        string content)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(filePath), filePath);
        Guard.IsNotNullOrWhiteSpace(nameof(content), content);

        await using var writer = File.CreateText(filePath);
        await writer.WriteAsync(content).ConfigureAwait(false);
    }

    /// <summary>
    /// Ecrit l'ensemble d'une liste de chaine de caractères dans un fichier.
    /// </summary>
    /// <param name="filePath">Chemin du fichier.</param>
    /// <param name="lines">Contenu du fichier sous forme de liste.</param>
    /// <returns>Tache asynchrone.</returns>
    public static async Task WriteAsync(string filePath,
                                        List<string> lines)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(filePath), filePath);
        Guard.IsNotNull(nameof(lines), lines);

        var content = string.Join(Environment.NewLine, lines.ToArray());
        await WriteAsync(filePath, content).ConfigureAwait(false);
    }

    /// <summary>
    /// Ecrit un stream dans un fichier.
    /// </summary>
    /// <param name="filePath">Chemin du fichier.</param>
    /// <param name="stream">Contenu du fichier.</param>
    /// <param name="cancellationToken">Token d’annulation</param>
    /// <returns>Tache asynchrone.</returns>
    /// <exception cref="SecurityException">L'appelant n'a pas l'autorisation requise.</exception>
    /// <exception cref="UnauthorizedAccessException">L'accès à <paramref name="filePath" /> est refusé.</exception>
    public static async Task WriteAsync(string filePath,
                                        Stream stream,
                                        CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(filePath), filePath);
        Guard.IsNotNull(nameof(stream), stream);

        var fi = new FileInfo(filePath);
        if (fi.DirectoryName != null)
        {
            DirectoryHelper.CreateDirectoryIfNotExist(fi.DirectoryName);
        }

        await using var fileStream = new FileStream(fi.FullName, FileMode.Create);
        await stream.CopyToAsync(fileStream, cancellationToken).ConfigureAwait(false);
        await fileStream.FlushAsync(cancellationToken);
        fileStream.Close();
    }

    public static async Task WriteAsync(string filePath,
                                        string content,
                                        Encoding encoding)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(filePath), filePath);
        Guard.IsNotNull(nameof(encoding), encoding);

        await using var writer = new StreamWriter(filePath, false, encoding);
        await writer.WriteAsync(content).ConfigureAwait(false);
    }

    /// <summary>
    /// Ecrit l'ensemble d'une chaine de caractères dans un fichier.
    /// </summary>
    /// <param name="filePath">Chemin du fichier.</param>
    /// <param name="base64">Contenu du fichier en base64.</param>
    /// <param name="cancellationToken">Token d’annulation</param>
    public static async Task WriteBase64Async(string filePath, string base64, CancellationToken cancellationToken)
    {
        var bytes = Convert.FromBase64String(base64);
        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write, BufferSize, true))
        {
            await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Ecrit l'ensemble d'une chaine de caractères dans un fichier.
    /// </summary>
    /// <param name="filePath">Chemin du fichier.</param>
    /// <param name="content">Contenu du fichier.</param>
    /// <param name="encoding">Encoding du fichier.</param>
    /// <param name="cancellationToken">Token d’annulation</param>
    public static async Task WriteTextAsync(string filePath, string content, Encoding encoding, CancellationToken cancellationToken)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var encodedText = encoding.GetBytes(content);

        using (var sourceStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, BufferSize, true))
        {
            await sourceStream.WriteAsync(encodedText, 0, encodedText.Length, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Ecrit l'ensemble d'une chaine de caractères dans un fichier.
    /// </summary>
    /// <param name="filePath">Chemin du fichier.</param>
    /// <param name="content">Contenu du fichier.</param>
    /// <param name="cancellationToken">Token d’annulation</param>
    public static async Task WriteTextAsync(string filePath, string content, CancellationToken cancellationToken)
    {
        await WriteTextAsync(filePath, content, EncodingHelper.GetEuropeOccidentale(), cancellationToken);
    }
}