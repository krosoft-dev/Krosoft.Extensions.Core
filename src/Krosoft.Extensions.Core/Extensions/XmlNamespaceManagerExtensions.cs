using System.Xml;

namespace Krosoft.Extensions.Core.Extensions;

public static class XmlNamespaceManagerExtensions
{
    public static void AddNamespaces(this XmlNamespaceManager nsmgr, params KeyValuePair<string, string>[] namespaces)
    {
        foreach (var ns in namespaces)
        {
            nsmgr.AddNamespace(ns.Key, ns.Value);
        }
    }
}