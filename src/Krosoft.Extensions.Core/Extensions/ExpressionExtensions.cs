using System.Linq.Expressions;

namespace Krosoft.Extensions.Core.Extensions;

/// <summary>
/// Méthodes d'extensions sur les expressions Linq.
/// </summary>
public static class ExpressionExtensions
{
    /// <summary>
    /// Récupère la valeur d'une MemberExpression.
    /// </summary>
    /// <param name="member">MemberExpression à requêter.</param>
    /// <returns>La valeur de la MemberExpression en paramètre.</returns>
    public static T GetValue<T>(this MemberExpression member)
    {
        var objectMember = Expression.Convert(member, typeof(T));

        var getterLambda = Expression.Lambda<Func<T>>(objectMember);

        var getter = getterLambda.Compile();

        return getter();
    }
}