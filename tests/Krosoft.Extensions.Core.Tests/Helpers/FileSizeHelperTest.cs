using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Testing;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class FileSizeHelperTest : BaseTest
{
    [TestMethod]
    public void ReadableFileSizeDoubleTest()
    {
        const double size1 = 2408160605549;
        var readableFileSize1 = FileSizeHelper.ReadableFileSize(size1);
        Assert.AreEqual("2,19 To", readableFileSize1);

        const double size2 = 186299173026.186299173026;
        var readableFileSize2 = FileSizeHelper.ReadableFileSize(size2);
        Assert.AreEqual("173,5 Go", readableFileSize2);
    }

    [TestMethod]
    public void ReadableFileSizeLongTest()
    {
        const long size1 = 2408160605549;
        var readableFileSize1 = FileSizeHelper.ReadableFileSize(size1);
        Assert.AreEqual("2,19 To", readableFileSize1);

        const long size2 = 186299173026;
        var readableFileSize2 = FileSizeHelper.ReadableFileSize(size2);
        Assert.AreEqual("173,5 Go", readableFileSize2);
    }
}