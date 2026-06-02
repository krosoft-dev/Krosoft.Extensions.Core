using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Core.Tests.Helpers;

[TestClass]
public class ExceptionHelperTests
{
    [TestMethod]
    public void ExtractErrors_Null_ReturnsEmptySet()
    {
        var result = ExceptionHelper.ExtractErrors(null);

        Check.That(result).IsEmpty();
    }

    [TestMethod]
    public void ExtractErrors_StandardException_ReturnsMessage()
    {
        var ex = new Exception("Une erreur inattendue.");

        var result = ExceptionHelper.ExtractErrors(ex);

        Check.That(result).ContainsExactly("Une erreur inattendue.");
    }

    [TestMethod]
    public void ExtractErrors_KrosoftTechnicalException_SingleError()
    {
        var ex = new KrosoftTechnicalException("Erreur technique.");

        var result = ExceptionHelper.ExtractErrors(ex);

        Check.That(result).ContainsExactly("Erreur technique.");
    }

    [TestMethod]
    public void ExtractErrors_KrosoftTechnicalException_MultipleErrors()
    {
        var errors = new HashSet<string> { "Erreur 1.", "Erreur 2.", "Erreur 3." };
        var ex = new KrosoftTechnicalException(errors);

        var result = ExceptionHelper.ExtractErrors(ex);

        Check.That(result).IsEquivalentTo(errors);
    }

    [TestMethod]
    public void ExtractErrors_KrosoftFunctionalException_SingleError()
    {
        var ex = new KrosoftFunctionalException("Erreur fonctionnelle.");

        var result = ExceptionHelper.ExtractErrors(ex);

        Check.That(result).ContainsExactly("Erreur fonctionnelle.");
    }

    [TestMethod]
    public void ExtractErrors_KrosoftFunctionalException_MultipleErrors()
    {
        var errors = new HashSet<string> { "Champ requis.", "Format invalide." };
        var ex = new KrosoftFunctionalException(errors);

        var result = ExceptionHelper.ExtractErrors(ex);

        Check.That(result).IsEquivalentTo(errors);
    }
}
