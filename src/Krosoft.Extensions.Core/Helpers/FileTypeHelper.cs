using System.Text;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Tools;

namespace Krosoft.Extensions.Core.Helpers;

public static class FileTypeHelper
{
    public static bool IsEdifact(byte[] fileBytes)
    {
        Guard.IsNotNull(nameof(fileBytes), fileBytes);

        if (fileBytes.Length < 9)
        {
            return false;
        }

        var fileContent = Encoding.ASCII.GetString(fileBytes, 0, Math.Min(fileBytes.Length, 128));

        // Un fichier EDIFACT commence généralement par "UNA" ou "UNB"
        if (fileContent.StartsWith("UNA") || fileContent.Contains("UNB+"))
        {
            // Format UNA : https://www.edi-plus.com/fr/faq/quest-ce-que-segment-una
            // Vérifie le segment UNA et son format typique
            if (fileContent.StartsWith("UNA"))
            {
                // Le segment UNA doit avoir exactement 9 caractères (UNA + 6 caractères de service)
                if (fileContent.Length < 9)
                {
                    return false;
                }

                // UNA suivi de 6 caractères de service
                return true;
            }

            // Format UNB : début d'un message EDIFACT sans segment UNA
            if (fileContent.Contains("UNB+"))
            {
                // Vérification supplémentaire pour UNB (identifiant de l'expéditeur, du destinataire, etc.)
                return fileContent.Contains("UNB+") && fileContent.Contains("UNH+");
            }
        }

        return false;
    }

    public static bool IsZip(byte[] fileBytes)
    {
        Guard.IsNotNull(nameof(fileBytes), fileBytes);

        return fileBytes.Length >= 4 &&
               fileBytes[0] == 0x50 &&
               fileBytes[1] == 0x4B &&
               fileBytes[2] == 0x03 &&
               fileBytes[3] == 0x04;
    }

    public static bool IsPdf(byte[] fileBytes)
    {
        Guard.IsNotNull(nameof(fileBytes), fileBytes);

        var fileHeader = Encoding.UTF8.GetString(fileBytes, 0, Math.Min(fileBytes.Length, 5));
        return fileHeader.StartsWith("%PDF-");
    }

    public static bool IsPdf(Stream stream) => IsPdf(ReadHeader(stream, 5));

    // Lit les premiers octets sans consommer le flux : la position d'origine est restaurée.
    private static byte[] ReadHeader(Stream stream, int length)
    {
        Guard.IsNotNull(nameof(stream), stream);

        if (!stream.CanSeek)
        {
            throw new KrosoftTechnicalException("Le flux doit être repositionnable pour en lire l'en-tête.");
        }

        var position = stream.Position;
        var header = new byte[length];
        var read = stream.ReadAtLeast(header, length, false);
        stream.Position = position;

        return header[..read];
    }

    /// <summary>
    /// Indique si le contenu ressemble à un document XML, d'après ses premiers caractères (le document n'est pas parsé).
    /// La déclaration "&lt;?xml ...?&gt;" étant optionnelle, un contenu commençant par un commentaire, un DOCTYPE
    /// ou un élément racine est aussi reconnu ; un document HTML l'est donc également.
    /// Les BOM UTF-8 et UTF-16 (LE/BE) sont pris en charge.
    /// </summary>
    public static bool IsXml(byte[] fileBytes)
    {
        Guard.IsNotNull(nameof(fileBytes), fileBytes);

        var content = DecodeTextHeader(fileBytes, 256).TrimStart(' ', '\t', '\r', '\n');
        if (content.Length < 2 || content[0] != '<')
        {
            return false;
        }

        return content.StartsWith("<?xml", StringComparison.Ordinal) ||
               content.StartsWith("<!--", StringComparison.Ordinal) ||
               content.StartsWith("<!DOCTYPE", StringComparison.Ordinal) ||
               char.IsLetter(content[1]) ||
               content[1] == '_' ||
               content[1] == ':';
    }

    /// <summary>
    /// Décode au plus <paramref name="maxByteCount" /> octets en texte, selon l'encodage indiqué par le BOM (UTF-8 par défaut).
    /// </summary>
    private static string DecodeTextHeader(byte[] bytes, int maxByteCount)
    {
        var (encoding, bomLength) = bytes switch
        {
            [0xEF, 0xBB, 0xBF, ..] => (Encoding.UTF8, 3),
            [0xFF, 0xFE, ..] => (Encoding.Unicode, 2),
            [0xFE, 0xFF, ..] => (Encoding.BigEndianUnicode, 2),
            _ => (Encoding.UTF8, 0)
        };

        return encoding.GetString(bytes, bomLength, Math.Min(bytes.Length - bomLength, maxByteCount));
    }

    private static int GetContentStartIndex(byte[] bytes)
    {
        var index = 0;

        // Ignorer les BOM.
        if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
        {
            index = 3; // UTF-8 BOM
        }
        else if (bytes.Length >= 2 && ((bytes[0] == 0xFF && bytes[1] == 0xFE) || (bytes[0] == 0xFE && bytes[1] == 0xFF)))
        {
            index = 2; // UTF-16 BOM
        }

        // Ignorer les espaces, tabs, retours à la ligne.
        while (index < bytes.Length && (bytes[index] == 0x20 || bytes[index] == 0x09 || bytes[index] == 0x0A || bytes[index] == 0x0D))
        {
            index++;
        }

        return index;
    }

    public static bool IsJson(byte[] fileBytes)
    {
        Guard.IsNotNull(nameof(fileBytes), fileBytes);

        var startIndex = GetContentStartIndex(fileBytes);
        if (startIndex >= fileBytes.Length)
        {
            return false;
        }

        if (fileBytes[startIndex] == 0x7B || fileBytes[startIndex] == 0x5B) // '{' ou '['
        {
            return true;
        }

        return false;
    }

    public static bool IsGZip(byte[] fileBytes)
    {
        Guard.IsNotNull(nameof(fileBytes), fileBytes);

        return fileBytes.Length >= 3 &&
               fileBytes[0] == 0x1F &&
               fileBytes[1] == 0x8B &&
               fileBytes[2] == 0x08;
    }
}