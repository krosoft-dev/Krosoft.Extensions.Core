using System.Xml;
using Krosoft.Extensions.Core.Models;

namespace Krosoft.Extensions.Core.Interfaces;

public interface IXmlLoaderService
{
    Result<XmlDocument> Load(KrosoftFile xmlFile);
}