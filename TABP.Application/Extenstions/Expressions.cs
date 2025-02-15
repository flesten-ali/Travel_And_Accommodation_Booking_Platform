using System.Linq.Expressions;

namespace TABP.Application.Extenstions;
public static class Expressions
{
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        var combinedBody = Expression.AndAlso(expr1.Body, Expression.Invoke(expr2, expr1.Parameters[0]));
        return Expression.Lambda<Func<T, bool>>(combinedBody, expr1.Parameters[0]);
    }
}