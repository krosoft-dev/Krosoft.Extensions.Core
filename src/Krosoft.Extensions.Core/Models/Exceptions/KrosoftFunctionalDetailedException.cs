namespace Krosoft.Extensions.Core.Models.Exceptions;

/// <summary>
/// Exception à retourner en cas d'erreur fonctionnelle soulevée par le code de l'application avec détails d'erreurs.
/// </summary>
public class KrosoftFunctionalDetailedException : KrosoftFunctionalException
{
    public KrosoftFunctionalDetailedException(string erreur,
                                              ISet<string> errors,
                                              ISet<ErrorDetail> errorDetails,
                                              Exception? innerException = null)
        : base(erreur, errors, innerException)
    {
        ErrorDetails = errorDetails;
    }

    public KrosoftFunctionalDetailedException(ISet<string> errors,
                                              ISet<ErrorDetail> errorDetails,
                                              Exception? innerException = null)
        : base(errors, innerException)
    {
        ErrorDetails = errorDetails;
    }

    public KrosoftFunctionalDetailedException(string erreur,
                                              ISet<ErrorDetail> errorDetails,
                                              Exception? innerException = null)
        : base(erreur, innerException)
    {
        ErrorDetails = errorDetails;
    }

    public IEnumerable<ErrorDetail> ErrorDetails { get; }
}