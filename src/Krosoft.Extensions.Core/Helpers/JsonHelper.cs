using System.Reflection;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Krosoft.Extensions.Core.Helpers;

public static class JsonHelper
{
    public static IEnumerable<JToken> AllTokens(JToken obj)
    {
        var toSearch = new Stack<JToken>(obj.Children());
        while (toSearch.Count > 0)
        {
            var inspected = toSearch.Pop();
            yield return inspected;
            foreach (var child in inspected)
            {
                toSearch.Push(child);
            }
        }
    }

    public static IEnumerable<T> Get<T>(Assembly assembly) => Get<T>(assembly, typeof(T).Name);

    public static IEnumerable<T> Get<T>(Assembly assembly, string fileName)
    {
        var results = AssemblyHelper.ReadFromAssembly<T>(assembly, $"{fileName}.json");
        return results;
    }

    public static bool IsValid(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        var json = input.Trim();

        if ((json.StartsWith('{') && json.EndsWith('}')) || //For object
            (json.StartsWith('[') && json.EndsWith(']'))) //For array
        {
            try
            {
                JToken.Parse(json);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
        }

        return false;
    }

    public static JObject ReplacePath<T>(this JToken root, string path, T? newValue)
    {
        Guard.IsNotNull(nameof(root), root);
        Guard.IsNotNullOrWhiteSpace(nameof(path), path);

        if (EqualityComparer<T>.Default.Equals(newValue, default))
        {
            return (JObject)root;
        }

        var jNewValue = JToken.FromObject(newValue!);
        foreach (var value in root.SelectTokens(path).ToList())
        {
            if (value == root)
            {
                root = jNewValue;
            }
            else
            {
                value.Replace(jNewValue);
            }
        }

        if (root is not JObject jObject)
        {
            throw new KrosoftTechnicalException("Impossible de convertir en JObject.");
        }

        return jObject;
    }

    public static string? ToBase64(object? obj)
    {
        Guard.IsNotNull(nameof(obj), obj);
        var json = JsonConvert.SerializeObject(obj);
        var dataBase64 = Base64Helper.StringToBase64(json);
        return dataBase64;
    }
}