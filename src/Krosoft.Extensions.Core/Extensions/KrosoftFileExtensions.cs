using Krosoft.Extensions.Core.Models;

namespace Krosoft.Extensions.Core.Extensions;

public static class KrosoftFileExtensions
{
    public static MultipartFormDataContent ToMultipartFormDataContent(this KrosoftFile file)
    {
        var content = new MultipartFormDataContent();
        content.Add(new ByteArrayContent(file.Content), "file", file.Name);
        return content;
    }
}