namespace Krosoft.Extensions.Core.Helpers;

public static class FileSizeHelper
{
    public static string ReadableFileSize(long size, int unit = 0) => TransformFileSize(size, unit);

    public static string ReadableFileSize(double size, int unit = 0) => TransformFileSize(size, unit);

    private static string TransformFileSize(double size, int unit = 0)
    {
        string[] units = ["o", "Ko", "Mo", "Go", "To", "Po", "Eo", "Zo", "Yo"];

        while (size >= 1024)
        {
            size /= 1024;
            ++unit;
        }

        return $"{size:0.##} {units[unit]}";
    }
}