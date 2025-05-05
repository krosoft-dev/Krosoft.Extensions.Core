using System.Text;
using Krosoft.Extensions.Core.Helpers;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class CryptographyHelperTests
{
    [TestMethod]
    public void HashMd5_WithByteArrayInput_ShouldReturnExpectedHash()
    {
        var input = Encoding.UTF8.GetBytes("test");

        var result = CryptographyHelper.HashMd5(input);

        Check.That(result.ToHex(true)).IsEqualTo("098F6BCD4621D373CADE4E832627B4F6");
    }

    [TestMethod]
    public void HashMd5_WithEmptyInput_ShouldReturnExpectedHash()
    {
        var input = Array.Empty<byte>();

        var result = CryptographyHelper.HashMd5(input);

        Check.That(result.ToHex(true)).IsEqualTo("D41D8CD98F00B204E9800998ECF8427E");
    }

    [TestMethod]
    public void HashMd5_WithNullByteArrayInput_ShouldThrowArgumentNullException()
    {
        Check.ThatCode(() => CryptographyHelper.HashMd5((byte[])null!))
             .Throws<ArgumentNullException>();
    }

    [TestMethod]
    public void HashMd5_WithStringInput_ShouldReturnExpectedHash()
    {
        var input = "test";

        var result = CryptographyHelper.HashMd5(input);

        Check.That(result.ToHex(true)).IsEqualTo("098F6BCD4621D373CADE4E832627B4F6");
    }

    [TestMethod]
    public void HashSha1_WithByteArrayInput_ShouldReturnExpectedHash()
    {
        var input = Encoding.UTF8.GetBytes("test");

        var result = CryptographyHelper.HashSha1(input);

        Check.That(result.ToHex(true)).IsEqualTo("A94A8FE5CCB19BA61C4C0873D391E987982FBBD3");
    }

    [TestMethod]
    public void HashSha1_WithEmptyInput_ShouldReturnExpectedHash()
    {
        var input = Array.Empty<byte>();

        var result = CryptographyHelper.HashSha1(input);

        Check.That(result.ToHex(true)).IsEqualTo("DA39A3EE5E6B4B0D3255BFEF95601890AFD80709");
    }

    [TestMethod]
    public void HashSha256_WithEmptyInput_ShouldReturnExpectedHash()
    {
        var input = "Hello";

        var result = CryptographyHelper.HashSha256(input, "secret", "salt");

        Check.That(result).IsEqualTo("qaz6WBdkz6CqRmXoObOZvYmJrYBThR1WW/LZGNPV16Q=");
    }

    [TestMethod]
    public void HashSha1_WithNullByteArrayInput_ShouldThrowArgumentNullException()
    {
        Check.ThatCode(() => CryptographyHelper.HashSha1(null!))
             .Throws<ArgumentNullException>();
    }
}