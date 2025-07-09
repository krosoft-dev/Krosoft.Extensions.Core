using System.Text;
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

    public static bool IsXml(byte[] fileBytes)
    {
        Guard.IsNotNull(nameof(fileBytes), fileBytes);

        var startIndex = GetContentStartIndex(fileBytes);
        if (startIndex >= fileBytes.Length)
        {
            return false;
        }

        // Vérifier XML : "<?xml"
        if (HasMagicBytes(fileBytes, startIndex, [0x3C, 0x3F, 0x78, 0x6D, 0x6C]))
        {
            return true;
        }

        return false;
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

    private static bool HasMagicBytes(byte[] fileBytes, int startIndex, byte[] magicBytes)
    {
        if (startIndex + magicBytes.Length > fileBytes.Length)
        {
            return false;
        }

        for (var i = 0; i < magicBytes.Length; i++)
        {
            if (fileBytes[startIndex + i] != magicBytes[i])
            {
                return false;
            }
        }

        return true;
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