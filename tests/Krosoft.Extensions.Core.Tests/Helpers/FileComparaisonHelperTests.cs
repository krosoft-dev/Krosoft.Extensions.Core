using System.Reflection;
using Krosoft.Extensions.Core.Helpers;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class FileComparaisonHelperTests
{
    [TestMethod]
    public async Task ComputeFileHash_Ok()
    {
        const string filePath = "hello.txt";
        await FileHelper.WriteTextAsync(filePath, "Hello World", CancellationToken.None);
        Check.That(File.Exists(filePath)).IsTrue();

        var sourceStream = AssemblyHelper.ReadAsStream(Assembly.GetExecutingAssembly(),
                                                       "sample.txt",
                                                       EncodingHelper.GetEuropeOccidentale());

        var generatedHash = FileComparaisonHelper.ComputeFileHash(filePath);
        var expectedHash = FileComparaisonHelper.ComputeFileHash(sourceStream.ToArray());
        Check.That(generatedHash).IsEqualTo(expectedHash);
    }
}