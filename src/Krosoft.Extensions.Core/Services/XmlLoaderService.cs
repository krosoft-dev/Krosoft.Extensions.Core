using System.Xml;
using Krosoft.Extensions.Core.Interfaces;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Tools;

namespace Krosoft.Extensions.Core.Services;

internal class XmlLoaderService : IXmlLoaderService
{
    public Result<XmlDocument> Load(KrosoftFile file)
    {
        Guard.IsNotNull(nameof(file), file);

        if (file.Content.Length == 0)
        {
            return Result<XmlDocument>.Failure(new KrosoftTechnicalException($"Le fichier '{file.Name}' est vide."));
        }

        var xmlDoc = new XmlDocument();
        using var stream = new MemoryStream(file.Content);

        try
        {
            xmlDoc.Load(stream);
        }
        catch (Exception ex)
        {
            return Result<XmlDocument>.Failure(new KrosoftTechnicalException($"Erreur lors de la lecture du fichier '{file.Name}', message : {ex.Message}"));
        }
        
        return Result<XmlDocument>.Success(xmlDoc);
    }
}