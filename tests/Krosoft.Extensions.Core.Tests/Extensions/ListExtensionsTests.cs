using System.Collections.ObjectModel;
using Krosoft.Extensions.Core.Extensions;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class ListExtensionsTests
{
    private readonly string[] _data = { "apple", "orange", "banana" };

    [TestMethod]
    public void AddRange_Integer_CheckContains()
    {
        var list = new List<int> { 1, 2, 3 };

        list.AddRange(new List<int> { 3, 4, 5 }, true);

        Check.That(list).ContainsExactly(1, 2, 3, 4, 5);
    }

    [TestMethod]
    public void AddRange_Integer_NotCheckContains()
    {
        var list = new List<int> { 1, 2, 3, 8, 9, 8, 9 };

        list.AddRange(new List<int> { 3, 4, 5 }, false);

        Check.That(list).ContainsExactly(1, 2, 3, 8, 9, 8, 9, 3, 4, 5);
    }

    [TestMethod]
    public void AddRange_String_CheckContains()
    {
        var list = new List<string> { "apple", "aaa", "orange", "bbbb" };

        list.AddRange(new List<string> { "orange", "banana", "bbbb" }, true);

        Check.That(list).ContainsExactly("apple", "aaa", "orange", "bbbb", "banana");
    }

    [TestMethod]
    public void ToDictionary_ShouldReturnCorrectDictionary()
    {
        var result = _data.ToDictionary(x => x.Length, true);

        Check.That(result).IsEqualTo(new Dictionary<int, string> { { 5, "apple" }, { 6, "orange" } });
    }

    [TestMethod]
    public void ToDictionaryWithModifier_ShouldReturnCorrectDictionary()
    {
        Check.ThatCode(() => new List<string> { "apple", "orange", "apple", "orange", "banana" }.ToDictionary(x => x.Length.ToString(), x => x.ToUpper(), false))
             .Throws<ArgumentException>()
             .WithMessage("An item with the same key has already been added. Key: 5");
    }

    [TestMethod]
    public void ToDictionaryWithModifier_ShouldReturnCorrectDictionary_Distinct()
    {
        var result = new List<string> { "apple", "orange", "apple", "orange", "banana" }.ToDictionary(x => x.Length.ToString(), x => x.ToUpper(), true);

        Check.That(result).ContainsExactly(new Dictionary<string, string> { { "5", "apple" }, { "6", "orange" } });
    }

    [TestMethod]
    public void ToReadOnlyDictionary_ShouldReturnCorrectReadOnlyDictionary()
    {
        Check.ThatCode(() => _data.ToReadOnlyDictionary(x => x.Length, false))
             .Throws<ArgumentException>()
             .WithMessage("An item with the same key has already been added. Key: 6");
    }

    [TestMethod]
    public void ToReadOnlyDictionary_ShouldReturnCorrectReadOnlyDictionary_Distinct()
    {
        var result = _data.ToReadOnlyDictionary(x => x.Length, true);

        Check.That(result).IsEqualTo(new ReadOnlyDictionary<int, string>(new Dictionary<int, string> { { 5, "apple" }, { 6, "orange" } }));
    }

    [TestMethod]
    public void ToReadOnlyDictionary_WithDistinctKeys_ReturnsCorrectDictionary()
    {
        var result = _data.ToList().ToReadOnlyDictionary(x => x.Length, true);

        Check.That(result).IsEqualTo(new ReadOnlyDictionary<int, string>(new Dictionary<int, string> { { 5, "apple" }, { 6, "orange" } }));
    }
}
 