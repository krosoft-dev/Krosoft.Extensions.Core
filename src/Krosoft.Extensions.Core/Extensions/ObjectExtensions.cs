using System.ComponentModel;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Krosoft.Extensions.Core.Helpers;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Extensions;

/// <summary>
/// Méthodes d'extension pour la classe <see cref="object" /> et ses classes enfants.
/// </summary>
public static class ObjectExtensions
{
    public static T? DeepCopy<T>(this T self)
    {
        var serialized = JsonConvert.SerializeObject(self);
        return JsonConvert.DeserializeObject<T>(serialized);
    }

    private static List<string> GetPropertiesExclusions<T>(Expression<Func<T, object>>[]? expressionsPropertiesExclusions)
    {
        var propertiesExclusions = new List<string>();
        if (expressionsPropertiesExclusions != null && expressionsPropertiesExclusions.Any())
        {
            foreach (var expression in expressionsPropertiesExclusions)
            {
                propertiesExclusions.Add(PropertyNameHelper.For(expression));
            }
        }

        return propertiesExclusions;
    }

    private static List<string> GetPropertiesInclusions(PropertyInfo[] properties, List<string> propertiesExclusions, Type t)
    {
        var propertiesInclusions = new List<string>();
        foreach (var propertyInfo in properties)
        {
            if (!propertiesExclusions.Contains(propertyInfo.Name))
            {
                var property = t.GetProperty(propertyInfo.Name);
                if (property != null)
                {
                    var attributes = (DescriptionAttribute[]?)property.GetCustomAttributes(typeof(DescriptionAttribute), false);

                    var columnName = attributes != null && attributes.Length > 0
                        ? attributes[0].Description
                        : propertyInfo.Name;

                    propertiesInclusions.Add(columnName);
                }
            }
        }

        return propertiesInclusions;
    }

    /// <summary>
    /// Affiche l'ensemble des propriétés
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="o"></param>
    /// <returns></returns>
    public static string? GetPropertiesToString<T>(this T? o) where T : class
    {
        if (o == null)
        {
            return null;
        }

        var propertyInfos = typeof(T).GetProperties();

        var sb = new StringBuilder();

        foreach (var info in propertyInfos)
        {
            object? value = "(null)";
            try
            {
                if (info.GetValue(o, null) != null)
                {
                    value = info.GetValue(o, null);
                }
            }
            catch (Exception)
            {
                // ignored
            }

            sb.AppendLine(info.Name + ": " + value);
        }

        return sb.ToString();
    }

    public static void SetPropertyValue<T, TProperty>(this T? obj,
                                                      Expression<Func<T, TProperty>> entityExpression,
                                                      TProperty newValueEntity) where T : new()
    {
        if (obj == null)
        {
            obj = new T();
        }

        var memberExpression = (MemberExpression)entityExpression.Body;
        var property = (PropertyInfo)memberExpression.Member;

        property.SetValue(obj, newValueEntity, null);
    }

    /// <summary>
    /// Transforme une liste d'objet en csv en se basant sur les propriétés de l'objet.
    /// </summary>
    /// <typeparam name="T">Type des objets.</typeparam>
    /// <param name="objectlist">Liste d'objet à transformer</param>
    /// <param name="separator">Séparateur de colonne.</param>
    /// <param name="expressionsPropertiesExclusions">Expression </param>
    /// <returns>Le csv généré à partir de la listE.</returns>
    public static string ToCsv<T>(this IEnumerable<T> objectlist,
                                  string separator,
                                  params Expression<Func<T, object>>[]? expressionsPropertiesExclusions)
    {
        var t = typeof(T);
        var properties = t.GetProperties();
        var propertiesExclusions = GetPropertiesExclusions(expressionsPropertiesExclusions);
        var propertiesInclusions = GetPropertiesInclusions(properties, propertiesExclusions, t);

        var header = string.Join(separator, propertiesInclusions.ToArray());
        var csvdata = new StringBuilder();
        csvdata.AppendLine(header);

        foreach (var o in objectlist)
        {
            var s = new StringBuilder();

            foreach (var propertyInfo in properties)
            {
                if (!propertiesExclusions.Contains(propertyInfo.Name))
                {
                    if (s.Length > 0)
                    {
                        s.Append(separator);
                    }

                    var x = propertyInfo.GetValue(o);

                    if (x != null)
                    {
                        s.Append(x);
                    }
                }
            }

            csvdata.AppendLine(s.ToString());
        }

        return csvdata.ToString();
    }

    /// <summary>
    /// Transforme une liste d'objet en une DataTable.
    /// </summary>
    /// <typeparam name="T">Type des objets de la liste.</typeparam>
    /// <param name="objectlist">Liste des objets.</param>
    /// <param name="nomDataTable">Nom de la DataTable de sortie</param>
    /// <returns>Une DataTable.</returns>
    public static DataTable ToDataTable<T>(this IEnumerable<T> objectlist,
                                           string nomDataTable)
    {
        var t = typeof(T);
        var properties = t.GetProperties();
        var table = new DataTable(nomDataTable);
        foreach (var prop in properties)
        {
            table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        }

        foreach (var item in objectlist)
        {
            var row = table.NewRow();
            foreach (var propertyInfo in properties)
            {
                row[propertyInfo.Name] = propertyInfo.GetValue(item) ?? DBNull.Value;
            }

            table.Rows.Add(row);
        }

        return table;
    }

    public static IDictionary<string, string?> ToKeyValue(this object? metaToken)
    {
        if (metaToken == null)
        {
            return new Dictionary<string, string?>();
        }

        var propertyInfos = metaToken.GetType()
                                     .GetProperties(BindingFlags.Instance | BindingFlags.Public);
        var token = propertyInfos
            .ToDictionary(prop => prop.Name, prop =>
            {
                var value = prop.GetValue(metaToken, null);
                if (value != null)
                {
                    return value.ToString();
                }

                return string.Empty;
            });

        return token;
    }
}