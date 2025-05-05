using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Testing;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class StringHelperTests : BaseTest
{
    [DataTestMethod]
    [DataRow("test", "01/01/0001")]
    [DataRow(null!, "01/01/0001")]
    [DataRow("", "01/01/0001")]
    [DataRow("12/08/1988", "12/08/1988")]
    [DataRow("2012-04-23T18:25:43.511Z", "23/04/2012")]
    public void FormatDateStringIncorrectTest(string input, string expected)
    {
        var formatDate = StringHelper.FormatDate(input);
        Check.That(formatDate).IsEqualTo(expected);
    }

    [DataTestMethod]
    [DataRow("Hello, World!", "Hello, World!")]
    [DataRow("", "")]
    [DataRow(null, null)]
    public void GenerateStreamFromString_Tests(string? input, string? expectedContent)
    {
        var resultStream = StringHelper.GenerateStreamFromString(input);

        if (expectedContent == null)
        {
            Check.That(resultStream.Length).IsEqualTo(0);
        }
        else
        {
            using var reader = new StreamReader(resultStream);
            var resultContent = reader.ReadToEnd();
            Check.That(resultContent).IsEqualTo(expectedContent);
        }
    }

    [DataTestMethod]
    [DataRow(null, "")]
    [DataRow("", "")]
    [DataRow("T", "T")]
    [DataRow("T ", "T")]
    [DataRow(" T", "T")]
    [DataRow(" T ", "T")]
    [DataRow("au", "AU")]
    [DataRow("AU", "AU")]
    [DataRow("Au", "AU")]
    [DataRow("Thibault de Montaigu", "TD")]
    [DataRow("thibault De montaigu", "TD")]
    [DataRow("Thibault de Montaigu de Lyon en Lausane", "TD")]
    [DataRow("Admin", "AD")]
    [DataRow("Administrator", "AD")]
    [DataRow("administrator", "AD")]
    [DataRow("AdministratorAuService", "AA")]
    [DataRow("Pierre Louis-Calixte de la Comédie-Française", "PL")]
    [DataRow("Claire de La Rüe du Can de la Comédie-Française", "CD")]
    public void GetAbbreviation_Tests(string input, string expected)
    {
        var formatDate = StringHelper.GetAbbreviation(input);
        Check.That(formatDate).IsEqualTo(expected);
    }

    [DataTestMethod]
    [DataRow(null, null)]
    [DataRow("", "")]
    [DataRow("abc123def456", "123456")]
    [DataRow("12 34 56", "123456")]
    [DataRow("!@#$%^&*()_+", "")]
    public void KeepDigitsOnly_Tests(string input, string expectedOutput)
    {
        var result = StringHelper.KeepDigitsOnly(input);

        Check.That(result).IsEqualTo(expectedOutput);
    }

    [DataTestMethod]
    [DataRow(5)]
    [DataRow(10)]
    [DataRow(15)]
    public void RandomString_Tests(int length)
    {
        var result = StringHelper.RandomString(length);
        Check.That(result).IsNotNull();
        Check.That(result.Length).IsEqualTo(length);
        Check.That(result.All(char.IsLetterOrDigit)).IsTrue();
    }

    [DataTestMethod]
    [DataRow(null, 0)]
    [DataRow("", 0)]
    [DataRow("160519", 160519)]
    [DataRow("9432.0", 0)]
    [DataRow("16,667", 0)]
    [DataRow("42.42", 0)]
    [DataRow("   -322   ", -322)]
    [DataRow("+4302", 4302)]
    [DataRow("(100);", 0)]
    [DataRow("01FA", 0)]
    public void ToInteger_Tests(string input, int expected)
    {
        var formatDate = StringHelper.ToInteger(input);
        Check.That(formatDate).IsEqualTo(expected);
    }

    [DataTestMethod]
    [DataRow(null, "")]
    [DataRow("", "")]
    [DataRow("   test   ", "test")]
    [DataRow("test   ", "test")]
    [DataRow("     test   ", "test")]
    public void Trim_Tests(string input, string expected)
    {
        var formatDate = StringHelper.Trim(input);
        Check.That(formatDate).IsEqualTo(expected);
    }

    [DataTestMethod]
    [DataRow(null, 10, null)]
    [DataRow("", 10, "")]
    [DataRow("Short text", 20, "Short text")]
    [DataRow("This is a long text", 10, "This is a ...")]
    [DataRow("Another long text", 5, "Anoth...")]
    [DataRow("ABCDE", 3, "ABC...")]
    public void Truncate_Tests(string input, int maxLength, string expected)
    {
        var result = StringHelper.Truncate(input, maxLength);
        Check.That(result).IsEqualTo(expected);
    }
}