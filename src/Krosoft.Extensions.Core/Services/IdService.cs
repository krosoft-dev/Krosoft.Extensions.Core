using Krosoft.Extensions.Core.Interfaces;

namespace Krosoft.Extensions.Core.Services;

public class IdService : IIdService
{
    public Guid NewGuid() => Guid.NewGuid();

    public Guid NewGuidVersion7() => Guid.CreateVersion7();
}
