using Krosoft.Extensions.Core.Helpers;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class EnumHelperTests
{
    [TestMethod]
    public void ParseEnumOrDefault_Valid_String()
    {
        var result = EnumHelper.ParseEnumOrDefault("Value1", TestEnum.None);
        Check.That(result).IsEqualTo(TestEnum.Value1);
    }

    [TestMethod]
    public void ParseEnumOrDefault_Valid_Numeric_String()
    {
        var result = EnumHelper.ParseEnumOrDefault("2", TestEnum.None);
        Check.That(result).IsEqualTo(TestEnum.Value2);
    }

    [TestMethod]
    public void ParseEnumOrDefault_Invalid_String()
    {
        var result = EnumHelper.ParseEnumOrDefault("InvalidValue", TestEnum.None);
        Check.That(result).IsEqualTo(TestEnum.None);
    }

    [TestMethod]
    public void ParseEnumOrDefault_Null_String()
    {
        var result = EnumHelper.ParseEnumOrDefault(null, TestEnum.Value3);
        Check.That(result).IsEqualTo(TestEnum.Value3);
    }

    [TestMethod]
    public void ParseEnumOrDefault_Empty_String()
    {
        var result = EnumHelper.ParseEnumOrDefault(string.Empty, TestEnum.Value1);
        Check.That(result).IsEqualTo(TestEnum.Value1);
    }

    [TestMethod]
    public void ParseEnumOrDefault_Case_Insensitive()
    {
        var result = EnumHelper.ParseEnumOrDefault("value2", TestEnum.None);
        Check.That(result).IsEqualTo(TestEnum.None);
    }

    [TestMethod]
    public void ParseEnumOrDefault_Different_Default_Values()
    {
        var result1 = EnumHelper.ParseEnumOrDefault("InvalidValue", TestEnum.Value1);
        var result2 = EnumHelper.ParseEnumOrDefault("InvalidValue", TestEnum.Value2);
        var result3 = EnumHelper.ParseEnumOrDefault("InvalidValue", TestEnum.Value3);

        Check.That(result1).IsEqualTo(TestEnum.Value1);
        Check.That(result2).IsEqualTo(TestEnum.Value2);
        Check.That(result3).IsEqualTo(TestEnum.Value3);
    }

    [TestMethod]
    public void GetValues_Returns_All_Enum_Values()
    {
        var result = EnumHelper.GetValues<TestEnum>();
        Check.That(result).ContainsExactly(TestEnum.None, TestEnum.Value1, TestEnum.Value2, TestEnum.Value3);
    }

    [TestMethod]
    public void GetValues_Returns_All_Values_Count()
    {
        var result = EnumHelper.GetValues<TestEnum>().ToList();
        Check.That(result).HasSize(4);
    }

    [TestMethod]
    public void GetValues_Different_Enum_Type()
    {
        var result = EnumHelper.GetValues<AnotherEnum>();
        Check.That(result).ContainsExactly(AnotherEnum.Option1, AnotherEnum.Option2);
    }

    [TestMethod]
    public void GetValues_Non_Enum_Type_Throws_Exception()
    {
        Check.ThatCode(() => EnumHelper.GetValues<string>())
             .Throws<InvalidOperationException>();
    }

    [TestMethod]
    public void GetValues_Enum_Not_Empty()
    {
        var result = EnumHelper.GetValues<TestEnum>();
        Check.That(result).Not.IsEmpty();
    }

    [TestMethod]
    public void ParseEnumOrDefault_With_Zero_Value()
    {
        var result = EnumHelper.ParseEnumOrDefault("0", TestEnum.Value1);
        Check.That(result).IsEqualTo(TestEnum.None);
    }

    [TestMethod]
    public void ParseEnumOrDefault_Whitespace_String()
    {
        var result = EnumHelper.ParseEnumOrDefault("   ", TestEnum.Value2);
        Check.That(result).IsEqualTo(TestEnum.Value2);
    }

    private enum TestEnum
    {
        None = 0,
        Value1 = 1,
        Value2 = 2,
        Value3 = 3
    }

    private enum AnotherEnum
    {
        Option1 = 100,
        Option2 = 200
    }
}