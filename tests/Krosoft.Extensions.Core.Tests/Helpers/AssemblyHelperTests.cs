using System.IO.Compression;
using System.Reflection;
using Krosoft.Extensions.Core.Helpers;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class AssemblyHelperTests
{
    [TestMethod]
    public void ReadAsStreamAndReuseTest()
    {
        var file = AssemblyHelper.ReadAsStream(Assembly.GetExecutingAssembly(), "import_assets.zip", EncodingHelper.GetEuropeOccidentale());
        Check.That(file).IsNotNull();
        Check.That(file.Length).IsEqualTo(407935);
        Check.That(file.CanRead).IsTrue();

        using var archive = new ZipArchive(file);
        Check.That(archive).IsNotNull();
        Check.That(archive.Entries).IsNotNull();
        Check.That(archive.Entries).HasSize(12);
    }

    [TestMethod]
    public void ReadAsStreamTest()
    {
        var file = AssemblyHelper.ReadAsStream(Assembly.GetExecutingAssembly(), "sample1.pdf", EncodingHelper.GetEuropeOccidentale());
        Check.That(file).IsNotNull();
        Check.That(file.Length).IsEqualTo(13264);
        Check.That(file.CanRead).IsTrue();
    }

    [TestMethod]
    public void ReadAsString_Ok()
    {
        var file = AssemblyHelper.ReadAsString(Assembly.GetExecutingAssembly(), "sample1.pdf", EncodingHelper.GetEuropeOccidentale());
        Check.That(file).IsNotNull();
        Check.That(file.Length).IsEqualTo(13264);
        Check.That(file).StartsWith("%PDF-1.4");
    }
}