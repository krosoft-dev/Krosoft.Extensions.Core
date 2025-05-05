using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Samples.Library.Models.Enums;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class EnumExtensionsTests
{
    [DataTestMethod]
    [DataRow(SampleCode.One, "Description for Value1")]
    [DataRow(SampleCode.Two, "Description for Value2")]
    [DataRow(SampleCode.Three, "Description for Value3")]
    [DataRow(SampleCode.Four, "Description for Value4")]
    [DataRow(SampleCode.Five, "Five")]
    public void GetDescription_ShouldReturnCorrectDescription(SampleCode value, string expectedDescription)
    {
        var result = value.GetDescription();

        Check.That(result).IsEqualTo(expectedDescription);
    }

    [DataTestMethod]
    [DataRow(SampleCode.One, "Display Name for Value1")]
    [DataRow(SampleCode.Two, "Display Name for Value2")]
    [DataRow(SampleCode.Three, "Display Name for Value3")]
    [DataRow(SampleCode.Four, "Display Name for Value4")]
    [DataRow(SampleCode.Five, "Five")]
    public void GetDisplayName_ShouldReturnCorrectDisplayName(SampleCode value, string expectedDisplayName)
    {
        var result = value.GetDisplayName();

        Check.That(result).IsEqualTo(expectedDisplayName);
    }
}