using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Core.Helpers;

public static class XmlHelper
{
    public static T? Deserialize<T>(string? xml)
    {
        if (xml != null)
        {
            using var reader = new StringReader(xml);
            var serializer = new XmlSerializer(typeof(T));
            return (T?)serializer.Deserialize(reader);
        }

        return default;
    }

    public static T? Deserialize<T>(Stream? reader)
    {
        if (reader != null)
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T?)serializer.Deserialize(reader);
        }

        return default;
    }

    public static Result<XmlDocument> Load(MemoryStream fitStream, string rootName)
    {
        var xmlDocument = new XmlDocument();

        try
        {
            xmlDocument.Load(fitStream);
        }
        catch (Exception e)
        {
            return new KrosoftFunctionalException($"Le fichier '{rootName}' n'a pas pu être lu : {e.Message}", e);
        }

        var localName = xmlDocument.DocumentElement?.LocalName;
        if (localName != rootName)
        {
            return new KrosoftFunctionalException($"Le fichier n'est pas valide (élément racine attendu : '{rootName}', trouvé : '{localName}').");
        }

        return xmlDocument;
    }

    public static XDocument Load(byte[] bytes)
    {
        using var memoryStream = new MemoryStream(bytes);
        return XDocument.Load(memoryStream);
    }

    public static byte[] SaveToBytes(XDocument xDocument)
    {
        using var memoryStream = new MemoryStream();
        xDocument.Save(memoryStream);
        return memoryStream.ToArray();
    }

    public static T? Deserialize<T>(byte[] file)
    {
        var serializer = new XmlSerializer(typeof(T));
        using var reader = new MemoryStream(file);
        return (T?)serializer.Deserialize(reader);
    }

    public static byte[] SerializeToBytes<T>(T file, params KeyValuePair<string, XNamespace>[] namespaces)
    {
        var serializer = new XmlSerializer(typeof(T));
        var serializerNamespaces = new XmlSerializerNamespaces();
        foreach (var ns in namespaces)
        {
            serializerNamespaces.Add(ns.Key, ns.Value.ToString());
        }

        using var memoryStream = new MemoryStream();
        using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
        {
            serializer.Serialize(writer, file, serializerNamespaces);
            writer.Flush();
        }

        return memoryStream.ToArray();
    }
}