using System.Linq.Expressions;
using System.Reflection;

namespace Blog.PublicApi.Infrastructure.Paging;

public static class PropertyResolver
{
    public static string? GetPropertyName<T>(this T type, Expression<Func<T, object>> expression)
    {
        return GetPropertyName<T>(expression);
    }
    
    public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
    {
        var lambda = expression as LambdaExpression;
        MemberExpression? memberExpression;
        if (lambda.Body is UnaryExpression unaryExpression)
        {
            memberExpression = unaryExpression.Operand as MemberExpression;
        }
        else
        {
            memberExpression = lambda.Body as MemberExpression;
        }

        if (memberExpression == null)
        {
            throw new ArgumentException("Укажите лямбда выражение", nameof(expression));
        }

        var propertyInfo = memberExpression.Member as PropertyInfo;

        if (propertyInfo == null)
        {
            throw new ArgumentException("Укажите лямбда выражение на свойство", nameof(expression));
        }

        return propertyInfo.Name;
    }
}