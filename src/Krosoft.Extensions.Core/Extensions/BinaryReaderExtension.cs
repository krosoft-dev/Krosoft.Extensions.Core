namespace Krosoft.Extensions.Core.Extensions;

public static class BinaryReaderExtension
{
    /// <summary>
    /// Permet de lire tous les Bytes d'un BinaryReader
    /// </summary>
    /// <param name="reader">le BinaryReader à lire</param>
    /// <param name="bufferSize">la taille du buffer</param>
    /// <returns>le tableau de Byte correpondant au BinaryReader lu</returns>
    public static byte[] ReadAllBytes(this BinaryReader reader, int bufferSize = 4096)
    {
        using (var ms = new MemoryStream())
        {
            var buffer = new byte[bufferSize];
            int count;
            while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
            {
                ms.Write(buffer, 0, count);
            }

            return ms.ToArray();
        }
    }
}