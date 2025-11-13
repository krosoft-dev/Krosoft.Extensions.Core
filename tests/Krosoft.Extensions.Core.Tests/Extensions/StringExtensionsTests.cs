using Krosoft.Extensions.Core.Extensions;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class StringExtensionsTests
{
    [TestMethod]
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

    [TestMethod]
    [DataRow("", 5, "")]
    [DataRow(null, 5, null)]
    [DataRow("abcdefgh", 3, "abc")]
    [DataRow("xyz", 10, "xyz")]
    [DataRow("12345", 0, "")]
    [DataRow("text", -1, "")]
    public void Left_Test(string? input, int length, string? expectedOutput)
    {
        var result = input.Left(length);

        Check.That(result).IsEqualTo(expectedOutput);
    }

    [TestMethod]
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

    [TestMethod]
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

    [TestMethod]
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

    [TestMethod]
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

    [TestMethod]
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

    [TestMethod]
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

    [TestMethod]
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

    [TestMethod]
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

    [TestMethod]
    [DataRow("asdf.txt", null, "asdf.txt")]
    [DataRow("\"<>|:*?\\/.txt", null, "_________.txt")]
    [DataRow("yes_its_valid_~!@#$%^&()_+.txt", null, "yes_its_valid__.txt")]
    [DataRow("*_*.txt", "Yo", "Yo_Yo.txt")]
    public void Sanitize_Tests(string? input, string? replacement, string? expectedOutput)
    {
        var result = input.Sanitize(replacement);

        Check.That(result).IsEqualTo(expectedOutput);
    }

    [TestMethod]
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

    [TestMethod]
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

    [TestMethod]
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

    [TestMethod]
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

    [TestMethod]
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

    [TestMethod]
    public void ToSlug_WithSimpleText_ReturnsLowercaseSlug()
    {
        var input = "Hello World";

        var result = input.ToSlug();

        Check.That(result).IsEqualTo("hello-world");
    }

    [TestMethod]
    public void ToSlug_WithAccentedCharacters_RemovesAccents()
    {
        var input = "Café résumé naïve";

        var result = input.ToSlug();

        Check.That(result).IsEqualTo("cafe-resume-naive");
    }

    [TestMethod]
    public void ToSlug_WithSpecialCharacters_RemovesSpecialChars()
    {
        var input = "Hello@World! #Test $2024";

        var result = input.ToSlug();

        Check.That(result).IsEqualTo("helloworld-test-2024");
    }

    [TestMethod]
    public void ToSlug_WithMultipleSpaces_ReplacesWithSingleDash()
    {
        var input = "Hello    World     Test";

        var result = input.ToSlug();

        Check.That(result).IsEqualTo("hello-world-test");
    }

    [TestMethod]
    public void ToSlug_WithMultipleDashes_ConsolidatesToSingleDash()
    {
        var input = "Hello---World--Test";

        var result = input.ToSlug();

        Check.That(result).IsEqualTo("hello-world-test");
    }

    [TestMethod]
    public void ToSlug_WithLeadingAndTrailingSpaces_TrimsSpaces()
    {
        var input = "   Hello World   ";

        var result = input.ToSlug();

        Check.That(result).IsEqualTo("hello-world");
    }

    [TestMethod]
    public void ToSlug_WithLeadingAndTrailingDashes_TrimsDashes()
    {
        var input = "---Hello World---";

        var result = input.ToSlug();

        Check.That(result).IsEqualTo("hello-world");
    }

    [TestMethod]
    public void ToSlug_WithMixedCase_ConvertsToLowercase()
    {
        var input = "HeLLo WoRLd TeST";

        var result = input.ToSlug();

        Check.That(result).IsEqualTo("hello-world-test");
    }

    [TestMethod]
    public void ToSlug_WithNumbers_PreservesNumbers()
    {
        var input = "Article 123 Version 2024";

        var result = input.ToSlug();

        Check.That(result).IsEqualTo("article-123-version-2024");
    }

    [TestMethod]
    public void ToSlug_WithExistingDashes_PreservesDashes()
    {
        var input = "Hello-World-Test";

        var result = input.ToSlug();

        Check.That(result).IsEqualTo("hello-world-test");
    }

    [TestMethod]
    public void ToSlug_WithTextExceedingMaxLength_TruncatesToMaxLength()
    {
        var input = "This is a very long text that exceeds the maximum length";
        var maxLength = 25; // Ajusté pour tomber sur un mot complet

        var result = input.ToSlug(maxLength);

        Check.That(result.Length).IsLessOrEqualThan(maxLength);
        Check.That(result).IsEqualTo("this-is-a-very-long-text");
    }

    [TestMethod]
    public void ToSlug_WithTextExceedingDefaultMaxLength_TruncatesTo50Characters()
    {
        var input = "This is a very long text that definitely exceeds the default maximum length of fifty characters";

        var result = input.ToSlug();

        Check.That(result).IsEqualTo("this-is-a-very-long-text-that-definitely-exceeds-t");
    }

    [TestMethod]
    public void ToSlug_WithTruncationEndingWithSpace_TrimsTrailingSpace()
    {
        var input = "Hello World This Is A Test";
        var maxLength = 12; // Coupe après "Hello World "

        var result = input.ToSlug(maxLength);

        Check.That(result).Not.EndsWith("-");
        Check.That(result).IsEqualTo("hello-world");
    }

    [TestMethod]
    public void ToSlug_WithFrenchAccents_RemovesAllAccents()
    {
        var input = "À È Ì Ò Ù Â Ê Î Ô Û Ä Ë Ï Ö Ü Ç";

        var result = input.ToSlug();

        Check.That(result).IsEqualTo("a-e-i-o-u-a-e-i-o-u-a-e-i-o-u-c");
    }

    [TestMethod]
    public void ToSlug_WithEmojisAndSymbols_RemovesEmojisAndSymbols()
    {
        var input = "Hello 🎉 World ★ Test ♥";

        var result = input.ToSlug();

        Check.That(result).IsEqualTo("hello-world-test");
    }

    [TestMethod]
    public void ToSlug_WithUnderscores_RemovesUnderscores()
    {
        var input = "Hello_World_Test";

        var result = input.ToSlug();

        Check.That(result).IsEqualTo("helloworldtest");
    }

    [TestMethod]
    public void ToSlug_WithDots_RemovesDots()
    {
        var input = "Hello.World.Test";

        var result = input.ToSlug();

        Check.That(result).IsEqualTo("helloworldtest");
    }

    [TestMethod]
    public void ToSlug_WithAmpersand_RemovesAmpersand()
    {
        var input = "Hello & World";

        var result = input.ToSlug();

        Check.That(result).IsEqualTo("hello-world");
    }

    [TestMethod]
    public void ToSlug_WithNullString_ReturnsEmptyString()
    {
        string? input = null;

        var result = input!.ToSlug();

        Check.That(result).IsEmpty();
    }

    [TestMethod]
    public void ToSlug_WithEmptyString_ReturnsEmptyString()
    {
        var input = string.Empty;

        var result = input.ToSlug();

        Check.That(result).IsEmpty();
    }

    [TestMethod]
    public void ToSlug_WithWhitespaceOnly_ReturnsEmptyString()
    {
        var input = "     ";

        var result = input.ToSlug();

        Check.That(result).IsEmpty();
    }

    [TestMethod]
    public void ToSlug_WithOnlySpecialCharacters_ReturnsEmptyString()
    {
        var input = "@#$%^&*()";

        var result = input.ToSlug();

        Check.That(result).IsEmpty();
    }

    [TestMethod]
    public void ToSlug_WithRealWorldExample_GeneratesValidSlug()
    {
        var input = "Les 10 meilleures pratiques pour développer en C# (2024)";

        var result = input.ToSlug();

        Check.That(result).HasSize(50); // Vérifie la troncature par défaut
        Check.That(result).IsEqualTo("les-10-meilleures-pratiques-pour-developper-en-c-2");
    }

    [TestMethod]
    public void ToSlug_WithRealWorldExample_GeneratesValidSlugg()
    {
        var input = "Les 10 meilleures pratiques pour développer en C# (2024)";

        var result = input.ToSlug(100); // Pas de troncature

        Check.That(result).IsEqualTo("les-10-meilleures-pratiques-pour-developper-en-c-2024");
    }

    [TestMethod]
    public void ToSlug_WithURLLikeText_GeneratesCleanSlug()
    {
        var input = "https://example.com/article-test";

        var result = input.ToSlug();

        Check.That(result).IsEqualTo("httpsexamplecomarticle-test");
    }

    [TestMethod]
    public void ToSlug_WithCustomMaxLength_RespectsCustomLength()
    {
        var input = "This is a test text";
        var maxLength = 10;

        var result = input.ToSlug(maxLength);

        Check.That(result.Length).IsLessOrEqualThan(maxLength);
        Check.That(result).IsEqualTo("this-is-a");
    }

    [TestMethod]
    public void ToSlug_WithMaxLengthZero_ReturnsEmptyString()
    {
        var input = "Hello World";
        var maxLength = 0;

        var result = input.ToSlug(maxLength);

        Check.That(result).IsEmpty();
    }

    [TestMethod]
    public void ToSlug_WithMaxLengthOne_ReturnsOneCharacter()
    {
        var input = "Hello World";
        var maxLength = 1;

        var result = input.ToSlug(maxLength);

        Check.That(result).IsEqualTo("h");
    }

    [TestMethod]
    public void ToSlug_WithSpanishAccents_RemovesSpanishAccents()
    {
        var input = "Español ñ á é í ó ú";

        var result = input.ToSlug();

        Check.That(result).IsEqualTo("espanol-n-a-e-i-o-u");
    }

    [TestMethod]
    public void ToSlug_WithGermanCharacters_RemovesGermanCharacters()
    {
        var input = "Österreich Über ß";

        var result = input.ToSlug();

        Check.That(result).IsEqualTo("osterreich-uber");
    }

    [TestMethod]
    public void ToSlug_WithConsecutiveSpacesAndDashes_ConsolidatesCorrectly()
    {
        var input = "Hello  --  World  --  Test";

        var result = input.ToSlug();

        Check.That(result).IsEqualTo("hello-world-test");
    }

    [TestMethod]
    public void ToSlug_WithTabsAndNewlines_ReplacesWithDashes()
    {
        var input = "Hello\tWorld\nTest\rEnd";

        var result = input.ToSlug();

        Check.That(result).IsEqualTo("hello-world-test-end");
    }
}