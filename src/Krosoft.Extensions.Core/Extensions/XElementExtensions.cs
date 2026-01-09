using System.Xml.Linq;

namespace Krosoft.Extensions.Core.Extensions;

public static class XElementExtensions
{
    extension(XElement element)
    {
        public XElement? ElementIgnoreCase(XName name)
        {
            return element.Elements()
                          .FirstOrDefault(e => e.Name.LocalName.Equals(name.LocalName, StringComparison.OrdinalIgnoreCase) && e.Name.Namespace == name.Namespace);
        }

        public IEnumerable<XElement> ElementsIgnoreCase(XName name)
        {
            return element.Elements()
                          .Where(e => e.Name.LocalName.Equals(name.LocalName, StringComparison.OrdinalIgnoreCase) && e.Name.Namespace == name.Namespace);
        }
    }
}