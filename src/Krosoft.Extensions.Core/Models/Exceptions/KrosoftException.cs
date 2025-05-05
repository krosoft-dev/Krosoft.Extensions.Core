#if !NET8_0_OR_GREATER
using System.Runtime.Serialization;
#endif

namespace Krosoft.Extensions.Core.Models.Exceptions;

public class KrosoftException : Exception
{
    public KrosoftException()
    {
    }

    public KrosoftException(string? message, Exception? innerException = null)
        : base(message, innerException)
    {
    }

#if !NET8_0_OR_GREATER
    public KrosoftException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
#endif
}