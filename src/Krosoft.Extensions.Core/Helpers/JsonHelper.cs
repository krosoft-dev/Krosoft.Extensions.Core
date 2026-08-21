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

    public static bool IsValid(string input) => TryParse(input, out _);

    /// <summary>
    ///     Normalise un JSON dans une forme strictement conforme à la RFC 8259 : caractères de contrôle échappés et
    ///     caractères NUL supprimés. Newtonsoft accepte à la lecture des séquences que des consommateurs stricts
    ///     (PostgreSQL json/jsonb, System.Text.Json...) rejettent : cette méthode les élimine.
    /// </summary>
    /// <returns>Le JSON normalisé, ou <c>null</c> si l'entrée n'est pas un objet ou un tableau JSON exploitable.</returns>
    public static string? Normalize(string? input)
    {
        if (!TryParse(input, out var token))
        {
            return null;
        }

        RemoveNullChars(token!);

        return token!.ToString(Formatting.None);
    }

    /// <summary>
    ///     Supprime les caractères NUL de toutes les valeurs texte du token : ils sont refusés par la plupart des
    ///     moteurs de stockage, y compris sous leur forme échappée.
    /// </summary>
    public static void RemoveNullChars(JToken token)
    {
        if (token is not JContainer container)
        {
            return;
        }

        foreach (var value in container.Descendants().OfType<JValue>().Where(v => v.Type == JTokenType.String))
        {
            value.Value = RemoveNullChars((string?)value.Value);
        }
    }

    public static string? RemoveNullChars(string? value) => value?.Replace("\0", string.Empty);

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

    private static bool TryParse(string? input, out JToken? token)
    {
        token = null;

        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        var json = input.Trim();

        if ((!json.StartsWith('{') || !json.EndsWith('}')) && //For object
            (!json.StartsWith('[') || !json.EndsWith(']'))) //For array
        {
            return false;
        }

        try
        {
            token = JToken.Parse(json);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}