using System.Security.Cryptography;
using System.Text;

namespace Krosoft.Extensions.Core.Helpers;

public static class CryptographyHelper
{
    public static byte[] HashMd5(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        return HashMd5(bytes);
    }

    public static byte[] HashMd5(byte[] input)
    {
        using var md5 = MD5.Create();
        return md5.ComputeHash(input);
    }

    public static byte[] HashSha1(byte[] input)
    {
        using var sha1 = SHA1.Create();
        return sha1.ComputeHash(input);
    }

    public static string HashSha256(string data, string secret, string salt)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret + salt));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return Convert.ToBase64String(hash);
    }
}