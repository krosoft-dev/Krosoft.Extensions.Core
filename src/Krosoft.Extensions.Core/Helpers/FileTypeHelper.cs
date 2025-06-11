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

        var fileHeader = Encoding.UTF8.GetString(fileBytes, 0, Math.Min(fileBytes.Length, 5));
        return fileHeader.StartsWith("<?xml");
    }

    public static bool IsJson(byte[] fileBytes)
    {
        Guard.IsNotNull(nameof(fileBytes), fileBytes);

        var fileHeader = Encoding.UTF8.GetString(fileBytes, 0, Math.Min(fileBytes.Length, 1));
        return fileHeader == "{" || fileHeader == "[";
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