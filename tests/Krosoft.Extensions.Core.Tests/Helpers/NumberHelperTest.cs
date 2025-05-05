using Krosoft.Extensions.Core.Helpers;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class NumberHelperTest
{
    [DataTestMethod]
    [DataRow("123.45", 123.45)]
    [DataRow("-789.12", -789.12)]
    [DataRow("42", 42)]
    [DataRow("abc", 0)]
    [DataRow(null, 0)]
    public void ToDecimal_ShouldConvertStringToDecimal(string input, double expectedResult)
    {
        var result = NumberHelper.ToDecimal(input);

        Check.That(result).IsEqualTo((decimal)expectedResult);
    }

    [DataTestMethod]
    [DataRow("123", 123)]
    [DataRow("-789", 789)]
    [DataRow("42", 42)]
    [DataRow("abc", 0)]
    [DataRow("160519", 160519)]
    [DataRow("9432.0", 9432)]
    [DataRow("16,667", 16)]
    [DataRow("42.42", 42)]
    [DataRow("   -322   ", 322)]
    [DataRow("+4302", 4302)]
    [DataRow("(100);", 100)]
    [DataRow("01FA", 1)]
    [DataRow("100 100", 100)]
    [DataRow("", 0)]
    [DataRow(null, 0)]
    public void ToInteger_ShouldConvertStringToInteger(string input, int expectedResult)
    {
        var result = NumberHelper.ToInteger(input);

        Check.That(result).IsEqualTo(expectedResult);
    }
}