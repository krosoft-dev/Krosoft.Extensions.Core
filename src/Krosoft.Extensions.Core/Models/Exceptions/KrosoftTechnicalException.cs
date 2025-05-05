using System.Net;
using Krosoft.Extensions.Core.Models.Exceptions.Http;

namespace Krosoft.Extensions.Core.Models.Exceptions;

/// <summary>
/// Exception à retourner en cas d'erreur technique soulevé par le code de l'application.
/// </summary>
public class KrosoftTechnicalException : HttpException
{
    public KrosoftTechnicalException(ISet<string> errors,
                                     Exception? innerException = null)
        : base(HttpStatusCode.InternalServerError, errors.FirstOrDefault(), innerException)
    {
        Errors = errors;
    }

    public KrosoftTechnicalException(string erreur,
                                     Exception? innerException = null) : this(new HashSet<string> { erreur }, innerException)
    {
    }

    public IEnumerable<string> Errors { get; }
}