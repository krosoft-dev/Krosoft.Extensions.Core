namespace Krosoft.Extensions.Core.Interfaces;

public interface IIdService
{
    Guid NewGuid();
    Guid NewGuidVersion7();
}
