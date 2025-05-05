using System.Text;

namespace Krosoft.Extensions.Core.Helpers;

public static class ByteHelper
{
    public static byte[] Combine(params byte[][] arrays)
    {
        var rv = new byte[arrays.Sum(a => a.Length)];
        var offset = 0;
        foreach (var array in arrays)
        {
            Buffer.BlockCopy(array, 0, rv, offset, array.Length);
            offset += array.Length;
        }

        return rv;
    }

    public static byte[] GetBytes(string str)
    {
        var bytes = new byte[str.Length * sizeof(char)];
        Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }

    public static string GetString(byte[] bytes) => GetString(bytes, EncodingHelper.GetEuropeOccidentale());

    public static string GetString(byte[] bytes, Encoding encoding) => encoding.GetString(bytes, 0, bytes.Length);

    public static string ToHex(this byte[] bytes, bool upperCase)
    {
        var result = new StringBuilder(bytes.Length * 2);

        for (var i = 0; i < bytes.Length; i++)
        {
            result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
        }

        return result.ToString();
    }
}