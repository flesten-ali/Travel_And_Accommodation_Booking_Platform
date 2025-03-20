using System.Linq.Expressions;

namespace TABP.Application.Extensions;

/// <summary>
/// Provides extension methods for working with LINQ expressions.
/// </summary>
public static class Expressions
{
    /// <summary>
    /// Combines two <see cref="Expression{Func{T, bool}}"/> using a logical AND.
    /// </summary>
    /// <typeparam name="T">The type of the parameter in the expressions.</typeparam>
    /// <param name="expr1">The first expression.</param>
    /// <param name="expr2">The second expression to be combined with the first.</param>
    /// <returns>A new <see cref="Expression{Func{T, bool}}"/> that represents the logical AND of the two expressions.</returns>
    /// <example>
    /// var expr1 = (Expression<Func<MyClass, bool>>)x => x.Property1 > 10;
    /// var expr2 = (Expression<Func<MyClass, bool>>)x => x.Property2 < 5;
    /// var combinedExpr = expr1.And(expr2);  // x => (x.Property1 > 10) && (x.Property2 < 5)
    /// </example>
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        var combinedBody = Expression.AndAlso(expr1.Body, Expression.Invoke(expr2, expr1.Parameters[0]));
        return Expression.Lambda<Func<T, bool>>(combinedBody, expr1.Parameters[0]);
    }
}