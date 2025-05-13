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
             .Throws<KrosoftTechnicalException>()
            ;
    }

    [TestMethod]
    public void IsNotNullOrEmptyKoEmptyTest()
    {
        var pdfFilepaths = new string[] { };
        Check.ThatCode(() => Guard.IsNotNullOrEmpty(nameof(pdfFilepaths), pdfFilepaths))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'pdfFilepaths' est vide ou non renseignée.");
    }

    [TestMethod]
    public void IsNotNullOrEmptyKoNotEmptyTest()
    {
        var pdfFilepaths = new string[] { };
        Check.ThatCode(() => Guard.IsNotNullOrEmpty(nameof(pdfFilepaths), pdfFilepaths))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'pdfFilepaths' est vide ou non renseignée.");
    }

    [TestMethod]
    public void IsNotNullOrEmptyOkTest()
    {
        var pdfFilepaths = new[] { "test", "test2" };
        Check.ThatCode(() => Guard.IsNotNullOrEmpty(nameof(pdfFilepaths), pdfFilepaths))
             .Not
             .Throws<KrosoftTechnicalException>();
    }

    [TestMethod]
    public void IsNotNullOrEmptyOkTest2()
    {
        var pdfFilepaths = new string[] { };
        Check.ThatCode(() => Guard.IsNotNullOrEmpty(nameof(pdfFilepaths), pdfFilepaths))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'pdfFilepaths' est vide ou non renseignée.");
    }

    [TestMethod]
    public void IsNotNullOrEmptyOkTest3()
    {
        var pdfFilepaths = new string?[] { null };
        Check.ThatCode(() => Guard.IsNotNullOrEmpty(nameof(pdfFilepaths), pdfFilepaths))
             .Not
             .Throws<KrosoftTechnicalException>();
    }

    [TestMethod]
    public void IsNotNullOrEmptyOkTest4()
    {
        var pdfFilepaths = new[] { " " };
        Check.ThatCode(() => Guard.IsNotNullOrEmpty(nameof(pdfFilepaths), pdfFilepaths))
             .Not
             .Throws<KrosoftTechnicalException>()
            ;
    }

    [TestMethod]
    public void IsNotNullOrEmptyOkTest5()
    {
        var pdfFilepaths = new[] { " ", null };
        Check.ThatCode(() => Guard.IsNotNullOrEmpty(nameof(pdfFilepaths), pdfFilepaths))
             .Not
             .Throws<KrosoftTechnicalException>()
            ;
    }

    [TestMethod]
    public void IsNotNullOrEmptyOkTest6()
    {
        var pdfFilepaths = new[] { "test", null };
        Check.ThatCode(() => Guard.IsNotNullOrEmpty(nameof(pdfFilepaths), pdfFilepaths))
             .Not
             .Throws<KrosoftTechnicalException>();
    }

    [TestMethod]
    public void IsNotNullOrEmptyOkTest7()
    {
        var pdfFilepaths = new List<string?> { "test", "test2", null };
        Check.ThatCode(() => Guard.IsNotNullOrEmpty(nameof(pdfFilepaths), pdfFilepaths))
             .Not
             .Throws<KrosoftTechnicalException>();
    }

    [TestMethod]
    public void IsNotNullOrEmptyOkTest78()
    {
        var pdfFilepaths = new List<string?>();
        Check.ThatCode(() => Guard.IsNotNullOrEmpty(nameof(pdfFilepaths), pdfFilepaths))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'pdfFilepaths' est vide ou non renseignée.");
    }

    [TestMethod]
    public void IsNotNullOrEmptyOkTest748()
    {
        List<string?>? pdfFilepaths = null;
        Check.ThatCode(() => Guard.IsNotNullOrEmpty(nameof(pdfFilepaths), pdfFilepaths))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'pdfFilepaths' est vide ou non renseignée.");
    }

    [TestMethod]
    public void IsNotNullOrEmptyOkTest8()
    {
        var id = 0;
        Check.ThatCode(() => Guard.IsNotNullOrEmpty(nameof(id), id))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'id' est vide ou non renseignée.");
    }

    [TestMethod]
    public void IsNotNullOrEmptyOkTest899()
    {
        long? id = null;
        Check.ThatCode(() => Guard.IsNotNullOrEmpty(nameof(id), id))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'id' est vide ou non renseignée.");
    }
}