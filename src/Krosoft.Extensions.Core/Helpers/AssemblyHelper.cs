using System.Reflection;
using System.Text;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Tools;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Helpers;

public static class AssemblyHelper
{
    private static string? GetResourceName(Assembly assembly, string filename)
    {
        var resourcesName = assembly.GetManifestResourceNames()
                                    .Where(s => s.EndsWith($".{filename}", StringComparison.CurrentCultureIgnoreCase))
                                    .ToList();
        if (resourcesName.Count > 1)
        {
            throw new KrosoftTechnicalException($"Plusieurs fichiers correspondent au fichier {filename} dans {assembly.GetName().Name}");
        }

        var resourceName = resourcesName.FirstOrDefault();
        return resourceName;
    }

    public static Stream Read(Assembly assembly,
                              string filename)
    {
        Guard.IsNotNull(nameof(assembly), assembly);
        Guard.IsNotNullOrWhiteSpace(nameof(filename), filename);

        var resourceName = GetResourceName(assembly, filename);
        if (string.IsNullOrEmpty(resourceName))
        {
            throw new KrosoftTechnicalException($"{filename} introuvable dans {assembly.GetName().Name}");
        }

        var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new KrosoftTechnicalException($"{resourceName} introuvable dans {assembly.GetName().Name}");
        }

        return stream;
    }

    public static MemoryStream ReadAsStream(Assembly assembly,
                                            string filename,
                                            Encoding encoding)
    {
        var data = ReadAsString(assembly, filename, encoding);
        var dataByte = encoding.GetBytes(data);
        var stream = new MemoryStream(dataByte);

        return stream;
    }

    public static string ReadAsString(Assembly assembly,
                                      string filename,
                                      Encoding encoding)
    {
        Guard.IsNotNull(nameof(assembly), assembly);
        Guard.IsNotNullOrWhiteSpace(nameof(filename), filename);

        var resourceName = GetResourceName(assembly, filename);
        if (string.IsNullOrEmpty(resourceName))
        {
            throw new KrosoftTechnicalException($"{filename} introuvable dans {assembly.GetName().Name}");
        }

        using (var stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null)
            {
                throw new KrosoftTechnicalException($"{resourceName} introuvable dans {assembly.GetName().Name}");
            }

            using (var streamReader = new StreamReader(stream, encoding, true))
            {
                return streamReader.ReadToEnd();
            }
        }
    }

    public static string ReadAsString(Assembly assembly,
                                      string resourceName) =>
        ReadAsString(assembly, resourceName, EncodingHelper.GetEuropeOccidentale());

    public static IEnumerable<string> ReadAsStringArray(Assembly assembly,
                                                        string resourceName)
    {
        Guard.IsNotNull(nameof(assembly), assembly);
        Guard.IsNotNullOrWhiteSpace(nameof(resourceName), resourceName);

        return ReadAsStringArray(assembly, resourceName, EncodingHelper.GetEuropeOccidentale());
    }

    public static IEnumerable<string> ReadAsStringArray(Assembly assembly,
                                                        string resourceName,
                                                        Encoding encoding)
    {
        Guard.IsNotNull(nameof(assembly), assembly);
        Guard.IsNotNullOrWhiteSpace(nameof(resourceName), resourceName);

        using (var stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null)
            {
                throw new KrosoftTechnicalException($"{resourceName} introuvable dans {assembly.GetName().Name}");
            }

            using (var streamReader = new StreamReader(stream, encoding, true))
            {
                string? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }

    public static IEnumerable<T> ReadFromAssembly<T>(Assembly assembly, string resourceName)
    {
        var json = ReadAsString(assembly, resourceName, EncodingHelper.GetEuropeOccidentale());
        var o = JsonConvert.DeserializeObject<IEnumerable<T>>(json);
        if (o == null)
        {
            throw new InvalidOperationException();
        }

        return o;
    }
}