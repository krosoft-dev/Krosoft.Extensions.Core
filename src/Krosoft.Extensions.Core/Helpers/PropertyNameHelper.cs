using System.Linq.Expressions;

namespace Krosoft.Extensions.Core.Helpers;

/// <summary>
/// Classe d'aide permettant d'obtenir le nom de propriétés à partir de lambda expressions.
/// </summary>
public static class PropertyNameHelper
{
    private static readonly IDictionary<string, string> PropertyNames = new Dictionary<string, string>();
    private static readonly object SyncLock = new object();

    /// <summary>
    /// Obtient le nom correspondant à une propriété.
    /// </summary>
    /// <param name="expression">Expression portant sur la propriété.</param>
    /// <returns>Nom correspondant.</returns>
    public static string For<T>(Expression<Func<T, object>> expression)
    {
        var key = $"{typeof(T).AssemblyQualifiedName}-{expression}";

        lock (SyncLock)
        {
            if (!PropertyNames.ContainsKey(key))
            {
                PropertyNames.Add(key, GetMemberName(expression.Body));
            }
        }

        lock (SyncLock)
        {
            return PropertyNames[key];
        }
    }

    private static string GetMemberName(Expression expression)
    {
        if (expression is MemberExpression memberExpression)
        {
            return memberExpression.Expression != null && memberExpression.Expression.NodeType == ExpressionType.MemberAccess
                ? string.Join(".", GetMemberName(memberExpression.Expression), memberExpression.Member.Name)
                : memberExpression.Member.Name;
        }

        if (expression is UnaryExpression unaryExpression)
        {
            if (unaryExpression.NodeType != ExpressionType.Convert)
            {
                throw new Exception($"Cannot interpret member from {expression}.");
            }

            return GetMemberName(unaryExpression.Operand);
        }

        throw new Exception($"Could not determine member from {expression}.");
    }
}