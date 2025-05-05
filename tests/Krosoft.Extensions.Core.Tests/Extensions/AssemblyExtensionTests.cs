using System.IO.Compression;
using System.Reflection;
using Krosoft.Extensions.Core.Extensions;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class AssemblyExtensionTests
{
    [TestMethod]
    public void ReadTest()
    {
        var file = Assembly.GetExecutingAssembly().Read("import_assets.zip");
        Check.That(file).IsNotNull();
        Check.That(file.Length).IsEqualTo(407935);
        Check.That(file.CanRead).IsTrue();

        using var archive = new ZipArchive(file);
        Check.That(archive).IsNotNull();
        Check.That(archive.Entries).IsNotNull();
        Check.That(archive.Entries).HasSize(12);
    }
}