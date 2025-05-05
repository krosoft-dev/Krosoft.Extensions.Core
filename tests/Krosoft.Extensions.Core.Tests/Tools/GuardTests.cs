using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Samples.Library.Models;

namespace Krosoft.Extensions.Core.Tests.Tools;

[TestClass]
public class GuardTests
{
    [TestMethod]
    public void IsNotNullKoTest()
    {
        Addresse? addresse = null;

        Check.ThatCode(() => Guard.IsNotNull(nameof(addresse), addresse))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'addresse' n'est pas renseignée.");
    }

    [TestMethod]
    public void IsNotNullOkTest()
    {
        var item = new Item();

        Check.ThatCode(() => Guard.IsNotNull(nameof(item), item))
             .Not.Throws<KrosoftTechnicalException>();
    }

    [TestMethod]
    public void IsNotNullOrWhiteSpaceKoEmptyTest()
    {
        var pdfFilepath = string.Empty;
        Check.ThatCode(() => Guard.IsNotNullOrWhiteSpace(nameof(pdfFilepath), pdfFilepath))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'pdfFilepath' est vide ou non renseignée.");
    }

    [TestMethod]
    public void IsNotNullOrWhiteSpaceKoWhiteSpaceTest()
    {
        var pdfFilepath = "      ";
        Check.ThatCode(() => Guard.IsNotNullOrWhiteSpace(nameof(pdfFilepath), pdfFilepath))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'pdfFilepath' est vide ou non renseignée.");
    }

    [TestMethod]
    public void IsNotNullOrWhiteSpaceOkTest()
    {
        var pdfFilepath = "test";
        Check.ThatCode(() => Guard.IsNotNullOrWhiteSpace(nameof(pdfFilepath), pdfFilepath))
             .Not
             .Throws<KrosoftTechnicalException>();
    }
}