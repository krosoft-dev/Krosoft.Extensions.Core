using System.Reflection;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class FileStreamExtensionsTests
{
    [TestMethod]
    public async Task ToBase64Async_Ok()
    {
        var fileName = "sample.txt";
        var stream = AssemblyHelper.ReadAsStream(Assembly.GetExecutingAssembly(),
                                                 fileName,
                                                 EncodingHelper.GetEuropeOccidentale());

        var fileStream = new GenericFileStream(stream, fileName, "text/plain");

        var base64 = await fileStream.ToBase64Async(CancellationToken.None);
        Check.That(base64).IsEqualTo("SGVsbG8gV29ybGQ=");
    }

    [TestMethod]
    public void ToBase64Async_Null()
    {
        Check.ThatCode(async () => { await FileStreamExtensions.ToBase64Async(null!, CancellationToken.None); })
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'file' n'est pas renseignée.")
             .And.WhichMember(x => x.Errors)
             .ContainsExactly("La variable 'file' n'est pas renseignée.");
    }
}