using System.Xml;
using Krosoft.Extensions.Core.Models;

namespace Krosoft.Extensions.Core.Interfaces;

/// <summary>
/// Service de chargement de documents XML à partir d'un fichier.
/// </summary>
public interface IXmlLoaderService
{
    /// <summary>
    /// Charge le contenu d'un fichier dans un <see cref="XmlDocument" />.
    /// </summary>
    /// <param name="xmlFile">Le fichier XML à charger.</param>
    /// <returns>
    /// Un <see cref="Result{XmlDocument}" /> contenant le document chargé en cas de succès,
    /// ou l'erreur rencontrée si le fichier est vide ou invalide.
    /// </returns>
    Result<XmlDocument> Load(KrosoftFile xmlFile);
}
