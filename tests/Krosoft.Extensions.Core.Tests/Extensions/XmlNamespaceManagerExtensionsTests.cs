using System.Xml;
using Krosoft.Extensions.Core.Extensions;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class XmlNamespaceManagerExtensionsTests
{
    private XmlNamespaceManager CreateManager() =>
        new XmlNamespaceManager(new NameTable());

    [TestMethod]
    public void AddNamespaces_SingleNamespace_Ok()
    {
        var nsmgr = CreateManager();

        nsmgr.AddNamespaces(new KeyValuePair<string, string>("ns", "http://example.com"));

        Check.That(nsmgr.LookupNamespace("ns")).IsEqualTo("http://example.com");
    }

    [TestMethod]
    public void AddNamespaces_MultipleNamespaces_Ok()
    {
        var nsmgr = CreateManager();

        nsmgr.AddNamespaces(
            new KeyValuePair<string, string>("a", "http://example.com/a"),
            new KeyValuePair<string, string>("b", "http://example.com/b"),
            new KeyValuePair<string, string>("c", "http://example.com/c")
        );

        Check.That(nsmgr.LookupNamespace("a")).IsEqualTo("http://example.com/a");
        Check.That(nsmgr.LookupNamespace("b")).IsEqualTo("http://example.com/b");
        Check.That(nsmgr.LookupNamespace("c")).IsEqualTo("http://example.com/c");
    }

    [TestMethod]
    public void AddNamespaces_NoNamespaces_DoesNotThrow()
    {
        var nsmgr = CreateManager();

        nsmgr.AddNamespaces();

        Check.That(nsmgr.LookupNamespace("any")).IsNull();
    }

    [TestMethod]
    public void AddNamespaces_DuplicatePrefix_OverwritesPrevious()
    {
        var nsmgr = CreateManager();

        nsmgr.AddNamespaces(
            new KeyValuePair<string, string>("ns", "http://example.com/v1"),
            new KeyValuePair<string, string>("ns", "http://example.com/v2")
        );

        Check.That(nsmgr.LookupNamespace("ns")).IsEqualTo("http://example.com/v2");
    }
}
