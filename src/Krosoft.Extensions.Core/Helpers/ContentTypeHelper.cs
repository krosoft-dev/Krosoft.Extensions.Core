using Krosoft.Extensions.Core.Tools;

namespace Krosoft.Extensions.Core.Helpers;

public static class ContentTypeHelper
{
    private static readonly Dictionary<string, string> ExtensionToContentType = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        { ".txt", "text/plain" },
        { ".pdf", "application/pdf" },
        { ".doc", "application/msword" },
        { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
        { ".xls", "application/vnd.ms-excel" },
        { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
        { ".png", "image/png" },
        { ".jpg", "image/jpeg" },
        { ".jpeg", "image/jpeg" },
        { ".gif", "image/gif" },
        { ".csv", "text/csv" },
        { ".html", "text/html" },
        { ".htm", "text/html" },
        { ".css", "text/css" },
        { ".js", "application/javascript" },
        { ".json", "application/json" },
        { ".mp3", "audio/mpeg" },
        { ".mp4", "video/mp4" },
        { ".zip", "application/zip" }
    };

    public static string GetContentType(string fileExtension)
    {
        Guard.IsNotNull(nameof(fileExtension), fileExtension);

        return ExtensionToContentType.GetValueOrDefault(fileExtension, "application/octet-stream");
    }
}