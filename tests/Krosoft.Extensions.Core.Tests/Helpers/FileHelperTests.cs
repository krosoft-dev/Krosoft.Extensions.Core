using Krosoft.Extensions.Core.Helpers;
using System.IO.Pipelines;
using System.Text;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class FileHelperTests
{
    [TestMethod]
    public async Task WriteTextAsync_Ok()
    {
        await FileHelper.WriteTextAsync("hello.txt", "Hello World", CancellationToken.None);
        var file = await FileHelper.ReadAsStringAsync("hello.txt", CancellationToken.None);

        Check.That(file).IsNotNull();
        Check.That(file.Length).IsEqualTo(11);
        Check.That(file).IsEqualTo("Hello World");
    }

    [TestMethod]
    public async Task ReadAsBytesAsync_Ok()
    {
        await FileHelper.WriteTextAsync("hello.txt", "Hello World", CancellationToken.None);

        var bytes = await FileHelper.ReadAsBytesAsync("hello.txt", CancellationToken.None);

        Check.That(bytes).IsNotNull();
        Check.That(bytes.Length).IsEqualTo(11);
        Check.That(bytes).IsEqualTo(new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100 });
    }

    [TestMethod]
    public async Task ReadAsBytes_Ok()
    {
        await FileHelper.WriteTextAsync("hello.txt", "Hello World", CancellationToken.None);

        var bytes = FileHelper.ReadAsBytes("hello.txt");

        Check.That(bytes).IsNotNull();
        Check.That(bytes.Length).IsEqualTo(11);
        Check.That(bytes).IsEqualTo(new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100 });
    }









 
    private string _testDirectory = null!;

    [TestInitialize]
    public void Setup()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDirectory);
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, true);
        }
    }

    [TestMethod]
    public async Task ReadAsStringArrayAsync_WithValidFile_ReturnsAllLines()
    {
        
        var filePath = Path.Combine(_testDirectory, "test.txt");
        var expectedLines = new[] { "Ligne 1", "Ligne 2", "Ligne 3" };
        await File.WriteAllLinesAsync(filePath, expectedLines, Encoding.UTF8);

         
        var result = await FileHelper.ReadAsStringArrayAsync(filePath, Encoding.UTF8);

        // Assert
        Check.That(result).ContainsExactly(expectedLines);
    }

    [TestMethod]
    public async Task ReadAsStringArrayAsync_WithEmptyFile_ReturnsEmptyCollection()
    {
        
        var filePath = Path.Combine(_testDirectory, "empty.txt");
        await File.WriteAllTextAsync(filePath, string.Empty);

         
        var result = await FileHelper.ReadAsStringArrayAsync(filePath, Encoding.UTF8);

        // Assert
        Check.That(result).IsEmpty();
    }

    [TestMethod]
    public async Task ReadAsStringArrayAsync_WithEmptyLines_IncludesEmptyLines()
    {
        
        var filePath = Path.Combine(_testDirectory, "with-empty-lines.txt");
        var expectedLines = new[] { "Ligne 1", "", "Ligne 3", "" };
        await File.WriteAllLinesAsync(filePath, expectedLines, Encoding.UTF8);

         
        var result = await FileHelper.ReadAsStringArrayAsync(filePath, Encoding.UTF8);

        // Assert
        Check.That(result).ContainsExactly(expectedLines);
    }

    [TestMethod]
    public async Task ReadAsStringArrayAsync_WithUtf8Encoding_ReadsSpecialCharacters()
    {
        
        var filePath = Path.Combine(_testDirectory, "utf8.txt");
        var expectedLines = new[] { "Héllo wørld", "Émoji: 🎉", "Çà marche!" };
        await File.WriteAllLinesAsync(filePath, expectedLines, Encoding.UTF8);

         
        var result = await FileHelper.ReadAsStringArrayAsync(filePath, Encoding.UTF8);

        // Assert
        Check.That(result).ContainsExactly(expectedLines);
    }

    [TestMethod]
    public async Task ReadAsStringArrayAsync_WithLatin1Encoding_ReadsCorrectly()
    {
        
        var filePath = Path.Combine(_testDirectory, "latin1.txt");
        var expectedLines = new[] { "Café", "Naïve", "Résumé" };
        await File.WriteAllLinesAsync(filePath, expectedLines, Encoding.Latin1);

         
        var result = await FileHelper.ReadAsStringArrayAsync(filePath, Encoding.Latin1);

        // Assert
        Check.That(result).ContainsExactly(expectedLines);
    }

    [TestMethod]
    public async Task ReadAsStringArrayAsync_WithLargeFile_ReadsAllLines()
    {
        
        var filePath = Path.Combine(_testDirectory, "large.txt");
        var expectedLines = Enumerable.Range(1, 10000)
            .Select(i => $"Ligne {i}")
            .ToArray();
        await File.WriteAllLinesAsync(filePath, expectedLines, Encoding.UTF8);

         
        var result = await FileHelper.ReadAsStringArrayAsync(filePath, Encoding.UTF8);

        // Assert
        Check.That(result).HasSize(10000);
        Check.That(result).ContainsExactly(expectedLines);
    }

    [TestMethod]
    public async Task ReadAsStringArrayAsync_WithSingleLine_ReturnsSingleElement()
    {
        
        var filePath = Path.Combine(_testDirectory, "single.txt");
        var expectedLine = "Une seule ligne";
        await File.WriteAllTextAsync(filePath, expectedLine, Encoding.UTF8);

         
        var result = await FileHelper.ReadAsStringArrayAsync(filePath, Encoding.UTF8);

        // Assert
        Check.That(result).ContainsExactly(expectedLine);
    }

    [TestMethod]
    public async Task ReadAsStringArrayAsync_WithTrailingNewLine_DoesNotAddEmptyLine()
    {
        
        var filePath = Path.Combine(_testDirectory, "trailing-newline.txt");
        await File.WriteAllTextAsync(filePath, "Ligne 1\nLigne 2\n", Encoding.UTF8);

         
        var result = await FileHelper.ReadAsStringArrayAsync(filePath, Encoding.UTF8);

        // Assert
        Check.That(result).HasSize(2);
        Check.That(result).ContainsExactly("Ligne 1", "Ligne 2");
    }

    [TestMethod] 
    public void ReadAsStringArrayAsync_WithNullFilePath_ThrowsArgumentException()
    {

        Check.ThatCode(() => FileHelper.ReadAsStringArrayAsync(null!, Encoding.UTF8))
            .Throws<FileNotFoundException>()
            .WithMessage("");
    }

    [TestMethod]
    public void ReadAsStringArrayAsync_WithEmptyFilePath_ThrowsArgumentException()
    {

        Check.ThatCode(() => FileHelper.ReadAsStringArrayAsync(string.Empty, Encoding.UTF8))
            .Throws<FileNotFoundException>()
            .WithMessage("La variable 'filePath' est vide ou non renseignée.");
    }

    [TestMethod]
    public void ReadAsStringArrayAsync_WithWhitespaceFilePath_ThrowsArgumentException()
    {

        Check.ThatCode(() => FileHelper.ReadAsStringArrayAsync("   ", Encoding.UTF8))
            .Throws<FileNotFoundException>()
            .WithMessage("");
    }

    [TestMethod]
    public void ReadAsStringArrayAsync_WithNonExistentFile_ThrowsFileNotFoundException()
    {
        
        var filePath = Path.Combine(_testDirectory, "nonexistent.txt");

 

        Check.ThatCode(() => FileHelper.ReadAsStringArrayAsync(filePath, Encoding.UTF8))
             .Throws<FileNotFoundException>()
             .WithMessage("");
    }

    [TestMethod]
    public async Task ReadAsStringArrayAsync_WithDifferentLineEndings_ReadsCorrectly()
    {
        
        var filePath = Path.Combine(_testDirectory, "mixed-endings.txt");
        // Windows (CRLF), Unix (LF), Old Mac (CR)
        var content = "Ligne 1\r\nLigne 2\nLigne 3\rLigne 4";
        await File.WriteAllTextAsync(filePath, content, Encoding.UTF8);

         
        var result = await FileHelper.ReadAsStringArrayAsync(filePath, Encoding.UTF8);

        // Assert
        Check.That(result).HasSize(4);
        Check.That(result.First()).IsEqualTo("Ligne 1");
        Check.That(result.Last()).IsEqualTo("Ligne 4");
    }

    [TestMethod]
    public async Task ReadAsStringArrayAsync_WithLongLines_ReadsCorrectly()
    {
        
        var filePath = Path.Combine(_testDirectory, "long-lines.txt");
        var longLine = new string('A', 10000);
        var expectedLines = new[] { longLine, "Ligne courte", longLine };
        await File.WriteAllLinesAsync(filePath, expectedLines, Encoding.UTF8);

         
        var result = await FileHelper.ReadAsStringArrayAsync(filePath, Encoding.UTF8);

        // Assert
        Check.That(result).HasSize(3);
        Check.That(result.ElementAt(0)).HasSize(10000);
        Check.That(result.ElementAt(1)).IsEqualTo("Ligne courte");
        Check.That(result.ElementAt(2)).HasSize(10000);
    }
}

 