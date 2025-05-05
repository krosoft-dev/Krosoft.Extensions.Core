using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Tools;

namespace Krosoft.Extensions.Core.Extensions;

public static class FileStreamExtensions
{
    public static async Task<string> ToBase64Async(this IFileStream file, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(nameof(file), file);

        using (var ms = new MemoryStream())
        {
            await file.Stream.CopyToAsync(ms, cancellationToken);
            var fileBytes = ms.ToArray();
            var s = Convert.ToBase64String(fileBytes);
            return s;
        }
    }
}