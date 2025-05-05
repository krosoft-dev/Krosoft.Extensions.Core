using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Core.Helpers;

public static class ExceptionHelper
{
    public static ISet<string> ExtractErrors(Exception? ex)
    {
        if (ex == null)
        {
            return new HashSet<string>();
        }

        if (ex is KrosoftTechnicalException technicalException)
        {
            return technicalException.Errors.ToHashSet();
        }

        if (ex is KrosoftFunctionalException functionalException)
        {
            return functionalException.Errors.ToHashSet();
        }

        return new HashSet<string> { ex.Message };
    }
}