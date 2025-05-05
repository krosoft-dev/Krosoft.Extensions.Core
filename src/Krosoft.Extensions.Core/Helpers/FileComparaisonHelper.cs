using System.Security.Cryptography;
using System.Text;

namespace Krosoft.Extensions.Core.Helpers;

public static class FileComparaisonHelper
{
    public static string ComputeFileHash(byte[] clearBytes)
    {
        using var sha1 = SHA1.Create();
        var hashedBytes = sha1.ComputeHash(clearBytes);
        return ConvertBytesToHex(hashedBytes);
    }

    public static string ComputeFileHash(string path) => ComputeFileHash(File.ReadAllBytes(path));

    public static string ConvertBytesToHex(byte[] bytes)
    {
        var sb = new StringBuilder();

        for (var i = 0; i < bytes.Length; i++)
        {
            sb.Append(bytes[i].ToString("x"));
        }

        return sb.ToString();
    }
}