using Krosoft.Extensions.Core.Extensions;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class DecimalExtensionTests
{
    [TestMethod]
    public void IsBetweenDecimalNullableNullParamTest()
    {
        var isBetween = DecimalExtension.IsBetween(null, 0, 10);
        Check.That(isBetween).IsFalse();
    }

    [TestMethod]
    public void IsBetweenDecimalNullableNullTest()
    {
        var isBetween = ((decimal?)null).IsBetween(0, 10);
        Check.That(isBetween).IsFalse();
    }

    [TestMethod]
    public void IsBetweenDecimalNullableOkTest()
    {
        decimal? number = 8;
        var isBetween = number.IsBetween(0, 10);
        Check.That(isBetween).IsTrue();
    }

    [TestMethod]
    public void IsBetweenDecimalOk1Test()
    {
        const decimal number = 8;
        var isBetween = number.IsBetween(0, 10);
        Check.That(isBetween).IsTrue();
    }

    [TestMethod]
    public void IsBetweenDecimalOk2Test()
    {
        const decimal number = 10;
        var isBetween = number.IsBetween(0, 10);
        Check.That(isBetween).IsTrue();
    }

    [TestMethod]
    public void IsBetweenDecimalOk3Test()
    {
        const decimal number = 2;
        var isBetween = number.IsBetween(2, 10);
        Check.That(isBetween).IsTrue();
    }

    [TestMethod]
    public void IsBetweenStrictDecimalNullableNullParamTest()
    {
        var isBetween = DecimalExtension.IsBetween(null, 0, 10, true);
        Check.That(isBetween).IsFalse();
    }

    [TestMethod]
    public void IsBetweenStrictDecimalNullableNullTest()
    {
        var isBetween = ((decimal?)null).IsBetween(0, 10, true);
        Check.That(isBetween).IsFalse();
    }

    [TestMethod]
    public void IsBetweenStrictDecimalNullableOkTest()
    {
        decimal? number = 8;
        var isBetween = number.IsBetween(0, 10, true);
        Check.That(isBetween).IsTrue();
    }

    [TestMethod]
    public void IsBetweenStrictDecimalOk1Test()
    {
        const decimal number = 8;
        var isBetween = number.IsBetween(0, 10, true);
        Check.That(isBetween).IsTrue();
    }

    [TestMethod]
    public void IsBetweenStrictDecimalOk2Test()
    {
        const decimal number = 10;
        var isBetween = number.IsBetween(0, 10, true);
        Check.That(isBetween).IsFalse();
    }

    [TestMethod]
    public void IsBetweenStrictDecimalOk3Test()
    {
        const decimal number = 2;
        var isBetween = number.IsBetween(2, 10, true);
        Check.That(isBetween).IsFalse();
    }
}