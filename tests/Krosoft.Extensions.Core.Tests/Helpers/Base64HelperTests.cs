using Krosoft.Extensions.Core.Helpers;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class Base64HelperTests
{
    [DataTestMethod]
    [DataRow("SGVsbG8sIFdvcmxkIQ==", "Hello, World!")]
    [DataRow("", "")]
    [DataRow(null, null)]
    public void Base64ToString(string base64EncodedData, string expectedPlainText)
    {
        var result = Base64Helper.Base64ToString(base64EncodedData);

        Check.That(result).IsEqualTo(expectedPlainText);
    }

    [DataTestMethod]
    [DataRow("and0VG9rZW4=", true)]
    [DataRow("aaa", false)]
    [DataRow("", false)]
    [DataRow(null, false)]
    public void IsBase64String(string base64EncodedData, bool expected)
    {
        var result = Base64Helper.IsBase64String(base64EncodedData);

        Check.That(result).IsEqualTo(expected);
    }

    [DataTestMethod]
    [DataRow("jwtToken", "and0VG9rZW4=")]
    [DataRow("", "")]
    [DataRow(null, null)]
    public void StringToBase64(string base64EncodedData, string expectedPlainText)
    {
        var result = Base64Helper.StringToBase64(base64EncodedData);

        Check.That(result).IsEqualTo(expectedPlainText);
    }
}