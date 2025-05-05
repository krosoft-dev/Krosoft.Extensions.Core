using Krosoft.Extensions.Core.Extensions;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class StringExtensionsTests
{
    [DataTestMethod]
    [DataRow("word", "word")] // Aucun changement attendu
    [DataRow("ThisIsAString", "This Is A String")] // Ajout d'espaces avant les majuscules
    [DataRow("CapitalStart", "Capital Start")] // Ajout d'espace après la première majuscule
    [DataRow("URLIsShortForUniformResourceLocator", "U R L Is Short For Uniform Resource Locator")] // Gestion de plusieurs majuscules consécutives
    [DataRow("", "")] // Chaîne vide
    [DataRow(null, "")] // Chaîne nulle
    [DataRow("alllowercase", "alllowercase")] // Aucun changement pour une chaîne sans majuscule
    [DataRow("One", "One")] // Pas de modification pour une seule majuscule
    [DataRow("Capital", "Capital")] // Pas d'espace avant la première majuscule
    public void AddSpacesBeforeCapitals(string input, string expected)
    {
        var result = input.AddSpacesBeforeCapitals();
        Check.That(result).IsEqualTo(expected);
    }

    [DataTestMethod]
    [DataRow("", 5, "")]
    [DataRow(null, 5, null)]
    [DataRow("", 5, "")]
    [DataRow("abcdefgh", 3, "abc")]
    [DataRow("xyz", 10, "xyz")]
    [DataRow("12345", 0, "")]
    [DataRow("text", -1, "")]
    public void Left_Test(string? input, int length, string? expectedOutput)
    {
        var result = input.Left(length);

        Check.That(result).IsEqualTo(expectedOutput);
    }

    [DataTestMethod]
    [DataRow(null, null, false)]
    [DataRow(null, "aaa", false)]
    [DataRow("aaa", null, false)]
    [DataRow("", "", false)]
    [DataRow("aaa", "", false)]
    [DataRow("", "aaa", false)]
    [DataRow("text", "un text court", true)]
    [DataRow("text", "un TEXT court", true)]
    [DataRow("text", "unTEXTcourt", true)]
    [DataRow("TEXT", "unTEXTcourt", true)]
    [DataRow("TEXT", "un text court", true)]
    [DataRow("un TEXT", "unTEXTcourt", true)]
    [DataRow("un TEXT", "un-TEXT-court", true)]
    [DataRow("un-TEXT-court", "un TEXT", true)]
    public void Match_Tests(string? searchText, string? text, bool expectedOutput)
    {
        var result = searchText.Match(text);
        Check.That(result).IsEqualTo(expectedOutput);
    }

    [DataTestMethod]
    [DataRow(null, null)]
    [DataRow("", "")]
    [DataRow("abc 123", "abc123")]
    [DataRow("   leading and trailing spaces   ", "leadingandtrailingspaces")]
    [DataRow("nochange", "nochange")]
    public void RemoveAllSpaces_Tests(string? input, string? expectedOutput)
    {
        var result = input.RemoveAllSpaces();
        Check.That(result).IsEqualTo(expectedOutput);
    }

    [TestMethod]
    public void RemoveDiacriticsTest()
    {
        Check.That("včľťšľžšžščýščýťčáčáčťáčáťýčťž".RemoveDiacritics()).IsEqualTo("vcltslzszscyscytcacactacatyctz");
        Check.That("Rez-de-chaussée".RemoveDiacritics()).IsEqualTo("Rez-de-chaussee");
    }

    [DataTestMethod]
    [DataRow(null, "", null)]
    [DataRow("", "", "")]
    [DataRow("test", null, "test")]
    [DataRow("TestString", "", "TestString")]
    [DataRow("TestString", "Test", "String")]
    [DataRow("TestString", null, "TestString")]
    [DataRow("TestString", "TestString", "")]
    [DataRow("TestString", "TestLongerPrefix", "TestString")] // prefix longer than input
    [DataRow("TestString", "String", "TestString")] // prefix not at the beginning
    [DataRow("Test String", "Test", " String")] // prefix with space in input
    [DataRow("Test@String!", "Test@", "String!")] // prefix with special characters
    [DataRow("testString", "Test", "testString")] // case sensitivity test
    public void RemovePrefix_Ok(string input, string prefix, string expectedOutput)
    {
        Check.That(input.RemovePrefix(prefix)).IsEqualTo(expectedOutput);
    }

    [DataTestMethod]
    [DataRow(null, null)]
    [DataRow("", "")]
    [DataRow("text", "text")]
    [DataRow("ét€", "t")]
    [DataRow("abc123", "abc123")]
    [DataRow("special!@#$characters", "specialcharacters")]
    [DataRow("remove spaces", "removespaces")]
    [DataRow("   leading and trailing spaces   ", "leadingandtrailingspaces")]
    public void RemoveSpecials_Tests(string input, string expectedOutput)
    {
        var result = input.RemoveSpecials();

        Check.That(result).IsEqualTo(expectedOutput);
    }

    [DataTestMethod]
    [DataRow(null, null)]
    [DataRow("", "")]
    [DataRow("http://example.com/", "http://example.com")]
    public void RemoveTrailingSlash_Ok(string input, string expectedOutput)
    {
        Check.That(input.RemoveTrailingSlash()).IsEqualTo(expectedOutput);
    }

    [TestMethod]
    public void Replace_Ok()
    {
        char[] separators = { ';', '.', ',' };
        var input = "this;is,a.test".Replace(separators, " ");
        Check.That(input).IsEqualTo("this is a test");
    }

    [DataTestMethod]
    [DataRow(null, "find", "replace", null)]
    [DataRow("", "find", "replace", "")]
    [DataRow("find and replace", "find", "new", "new and replace")]
    [DataRow("no match", "find", "replace", "no match")]
    [DataRow("replace first find and then find", "find", "Hello", "replace first Hello and then find")]
    public void ReplaceFirstOccurrence_Tests(string text, string seach, string replace, string expectedOutput)
    {
        var result = text.ReplaceFirstOccurrence(seach, replace);

        Check.That(result).IsEqualTo(expectedOutput);
    }

    [DataTestMethod]
    [DataRow(null, "find", "replace", null)]
    [DataRow("", "find", "replace", "")]
    [DataRow("find and replace", "find", "new", "new and replace")]
    [DataRow("no match", "find", "replace", "no match")]
    [DataRow("replace first find and then find", "find", "Hello", "replace first find and then Hello")]
    public void ReplaceLastOccurrence_Tests(string text, string seach, string replace, string expectedOutput)
    {
        var result = text.ReplaceLastOccurrence(seach, replace);

        Check.That(result).IsEqualTo(expectedOutput);
    }

    [DataTestMethod]
    [DataRow(null, 5, null)]
    [DataRow("", 5, "")]
    [DataRow("abcdefgh", 3, "fgh")]
    [DataRow("xyz", 10, "xyz")]
    [DataRow("12345", 0, "")]
    [DataRow("text", -1, "")]
    public void Right_Tests(string input, int length, string expectedOutput)
    {
        var result = input.Right(length);

        Check.That(result).IsEqualTo(expectedOutput);
    }

    [DataTestMethod]
    [DataRow("asdf.txt", null, "asdf.txt")]
    [DataRow("\"<>|:*?\\/.txt", null, "_________.txt")]
    [DataRow("yes_its_valid_~!@#$%^&()_+.txt", null, "yes_its_valid__.txt")]
    [DataRow("*_*.txt", "Yo", "Yo_Yo.txt")]
    public void Sanitize_Tests(string? input, string? replacement, string? expectedOutput)
    {
        var result = input.Sanitize(replacement);

        Check.That(result).IsEqualTo(expectedOutput);
    }

    [DataTestMethod]
    [DataRow(null, ' ', new string[] { })]
    [DataRow("", ' ', new string[] { })]
    [DataRow("   ", ' ', new string[] { })]
    [DataRow("abc def  ghi", ' ', new[] { "abc", "def", "ghi" })]
    [DataRow(" one, two , three ", ',', new[] { "one", "two", "three" })]
    [DataRow("   leading   and   trailing   ", ' ', new[] { "leading", "and", "trailing" })]
    public void SplitAndClean_Test(string? input, char splitString, string[] expectedOutput)
    {
        var result = input.SplitAndClean(splitString);

        Check.That(result).IsEqualTo(expectedOutput);
    }

    [DataTestMethod]
    [DataRow(null, null)]
    [DataRow("", "")]
    [DataRow("abc123", "abc123")]
    [DataRow("special!@#$characters", "specialcharacters")]
    [DataRow("remove spaces", "removespaces")]
    [DataRow("   leading and trailing spaces   ", "leadingandtrailingspaces")]
    public void ToAlphaNumeric_Tests(string? input, string expectedOutput)
    {
        var result = input.ToAlphaNumeric();

        Check.That(result).IsEqualTo(expectedOutput);
    }

    [DataTestMethod]
    [DataRow(null, 0)]
    [DataRow("", 0)]
    [DataRow("123", 123)]
    [DataRow("456", 456)]
    [DataRow("not a number", 0)]
    [DataRow("   789   ", 789)]
    public void ToInteger_Tests(string input, int expectedOutput)
    {
        var result = input.ToInteger();

        Check.That(result).IsEqualTo(expectedOutput);
    }

    [DataTestMethod]
    [DataRow(null, null)]
    [DataRow("", "")]
    [DataRow("abc", "Abc")]
    [DataRow("aBC", "Abc")]
    [DataRow("hello world", "Hello world")]
    [DataRow("   leading space", "   leading space")] // Should not change leading space
    [DataRow("123", "123")]
    public void ToUpperFirst_Tests(string? input, string? expectedOutput)
    {
        var result = input.ToUpperFirst();
        Check.That(result).IsEqualTo(expectedOutput);
    }

    [DataTestMethod]
    [DataRow(null, 10, null)]
    [DataRow("", 10, "")]
    [DataRow("abcdefghij", 5, "abcde")]
    [DataRow("xyz", 10, "xyz")]
    [DataRow("12345", 0, "")]
    [DataRow("text", -1, "text")]
    public void Truncate_Tests(string input, int maxLength, string expectedOutput)
    {
        var result = input.Truncate(maxLength);
        Check.That(result).IsEqualTo(expectedOutput);
    }
}

