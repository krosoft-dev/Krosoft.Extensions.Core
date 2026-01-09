using System.Xml.Linq;
using Krosoft.Extensions.Core.Extensions;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class XElementExtensionsTests
{
    private XElement CreateTestElement() =>
        XElement.Parse(@"
            <root>
                <Child name=""first"">Value1</Child>
                <child name=""second"">Value2</child>
                <CHILD name=""third"">Value3</CHILD>
                <Other>Value4</Other>
            </root>
        ");

    private XElement CreateTestElementWithNamespace() =>
        XElement.Parse(@"
            <root xmlns=""http://example.com"">
                <Child name=""first"">Value1</Child>
                <child name=""second"">Value2</child>
                <Other>Value4</Other>
            </root>
        ");

    private XElement CreateTestElementMultipleNamespaces() =>
        XElement.Parse(@"
            <root xmlns=""http://example.com"" xmlns:alt=""http://alternative.com"">
                <Child name=""first"">Value1</Child>
                <alt:Child name=""alt-first"">AltValue1</alt:Child>
                <child name=""second"">Value2</child>
            </root>
        ");

    [TestMethod]
    public void ElementIgnoreCase_Exact_Case_Match()
    {
        var root = CreateTestElement();
        var element = root.ElementIgnoreCase(XName.Get("Child"));

        Check.That(element).IsNotNull();
        Check.That(element?.Name.LocalName).IsEqualTo("Child");
        Check.That(element?.Attribute("name")?.Value).IsEqualTo("first");
    }

    [TestMethod]
    public void ElementIgnoreCase_Lower_Case_Input()
    {
        var root = CreateTestElement();
        var element = root.ElementIgnoreCase(XName.Get("child"));

        Check.That(element).IsNotNull();
        Check.That(element?.Name.LocalName).IsEqualTo("Child");
    }

    [TestMethod]
    public void ElementIgnoreCase_Upper_Case_Input()
    {
        var root = CreateTestElement();
        var element = root.ElementIgnoreCase(XName.Get("CHILD"));

        Check.That(element).IsNotNull();
        Check.That(element?.Name.LocalName).IsEqualTo("Child");
    }

    [TestMethod]
    public void ElementIgnoreCase_Non_Existent_Element()
    {
        var root = CreateTestElement();
        var element = root.ElementIgnoreCase(XName.Get("NonExistent"));

        Check.That(element).IsNull();
    }

    [TestMethod]
    public void ElementIgnoreCase_With_Namespace()
    {
        var root = CreateTestElementWithNamespace();
        var ns = XNamespace.Get("http://example.com");
        var element = root.ElementIgnoreCase(ns + "Child");

        Check.That(element).IsNotNull();
        Check.That(element?.Name.LocalName).IsEqualTo("Child");
    }

    [TestMethod]
    public void ElementIgnoreCase_With_Namespace_Case_Insensitive()
    {
        var root = CreateTestElementWithNamespace();
        var ns = XNamespace.Get("http://example.com");
        var element = root.ElementIgnoreCase(ns + "child");

        Check.That(element).IsNotNull();
        Check.That(element?.Name.LocalName).IsEqualTo("Child");
    }

    [TestMethod]
    public void ElementIgnoreCase_Wrong_Namespace()
    {
        var root = CreateTestElementWithNamespace();
        var wrongNs = XNamespace.Get("http://wrong.com");
        var element = root.ElementIgnoreCase(wrongNs + "Child");

        Check.That(element).IsNull();
    }

    [TestMethod]
    public void ElementIgnoreCase_Returns_First_Match()
    {
        var root = CreateTestElement();
        var element = root.ElementIgnoreCase(XName.Get("Child"));

        Check.That(element?.Attribute("name")?.Value).IsEqualTo("first");
    }

    [TestMethod]
    public void ElementsIgnoreCase_Exact_Case_Match()
    {
        var root = CreateTestElement();
        var elements = root.ElementsIgnoreCase(XName.Get("Child")).ToList();

        Check.That(elements).HasSize(3);
        Check.That(elements[0].Name.LocalName).IsEqualTo("Child");
    }

    [TestMethod]
    public void ElementsIgnoreCase_Lower_Case_Input()
    {
        var root = CreateTestElement();
        var elements = root.ElementsIgnoreCase(XName.Get("child")).ToList();

        Check.That(elements).HasSize(3);
    }

    [TestMethod]
    public void ElementsIgnoreCase_Upper_Case_Input()
    {
        var root = CreateTestElement();
        var elements = root.ElementsIgnoreCase(XName.Get("CHILD")).ToList();

        Check.That(elements).HasSize(3);
    }

    [TestMethod]
    public void ElementsIgnoreCase_Mixed_Case_Input()
    {
        var root = CreateTestElement();
        var elements = root.ElementsIgnoreCase(XName.Get("ChIlD")).ToList();

        Check.That(elements).HasSize(3);
    }

    [TestMethod]
    public void ElementsIgnoreCase_Non_Existent_Element()
    {
        var root = CreateTestElement();
        var elements = root.ElementsIgnoreCase(XName.Get("NonExistent"));

        Check.That(elements).IsEmpty();
    }

    [TestMethod]
    public void ElementsIgnoreCase_With_Namespace()
    {
        var root = CreateTestElementWithNamespace();
        var ns = XNamespace.Get("http://example.com");
        var elements = root.ElementsIgnoreCase(ns + "Child").ToList();

        Check.That(elements).HasSize(2);
    }

    [TestMethod]
    public void ElementsIgnoreCase_With_Namespace_Case_Insensitive()
    {
        var root = CreateTestElementWithNamespace();
        var ns = XNamespace.Get("http://example.com");
        var elements = root.ElementsIgnoreCase(ns + "child").ToList();

        Check.That(elements).HasSize(2);
    }

    [TestMethod]
    public void ElementsIgnoreCase_With_Wrong_Namespace()
    {
        var root = CreateTestElementWithNamespace();
        var wrongNs = XNamespace.Get("http://wrong.com");
        var elements = root.ElementsIgnoreCase(wrongNs + "Child");

        Check.That(elements).IsEmpty();
    }

    [TestMethod]
    public void ElementsIgnoreCase_Returns_All_Matching()
    {
        var root = CreateTestElement();
        var elements = root.ElementsIgnoreCase(XName.Get("child")).ToList();

        var attributeValues = elements.Select(e => e.Attribute("name")?.Value).ToList();
        Check.That(attributeValues).ContainsExactly("first", "second", "third");
    }

    [TestMethod]
    public void ElementsIgnoreCase_Empty_When_No_Match()
    {
        var root = CreateTestElement();
        var elements = root.ElementsIgnoreCase(XName.Get("DoesNotExist"));

        Check.That(elements).IsEmpty();
    }

    [TestMethod]
    public void ElementIgnoreCase_With_Different_Namespace_Prefixes()
    {
        var root = CreateTestElementMultipleNamespaces();
        var ns = XNamespace.Get("http://example.com");
        var element = root.ElementIgnoreCase(ns + "CHILD");

        Check.That(element).IsNotNull();
        Check.That(element?.Name.LocalName).IsEqualTo("Child");
    }

    [TestMethod]
    public void ElementsIgnoreCase_Respects_Namespace_Exact_Match()
    {
        var root = CreateTestElementMultipleNamespaces();
        var altNs = XNamespace.Get("http://alternative.com");
        var elements = root.ElementsIgnoreCase(altNs + "child").ToList();

        Check.That(elements).HasSize(1);
        Check.That(elements[0].Attribute("name")?.Value).IsEqualTo("alt-first");
    }

    [TestMethod]
    public void ElementIgnoreCase_Whitespace_Handling()
    {
        var root = XElement.Parse(@"<root><Child>Value</Child></root>");
        var element = root.ElementIgnoreCase(XName.Get("Child"));

        Check.That(element).IsNotNull();
    }
}