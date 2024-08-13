using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Blog.PublicApi.Infrastructure.Paging;

public static class PagingExtensions
{
    private static readonly List<Type> Collections = new() { typeof(IEnumerable<>), typeof(IEnumerable) };
    
    public static PagedResult<T> GetPagedResult<T>(this IQueryable<T> query, Paging<T> paging)
    {
        int count = query.Count();

        return new PagedResult<T>(query.SortAndPage(paging).ToArray(), count, paging);
    }
    
    public static async Task<PagedResult<T>> GetPagedResultAsync<T>(this IQueryable<T> query, Paging<T> paging)
    {
        int count = await query.CountAsync();

        return new PagedResult<T>(await query.SortAndPage(paging).ToListAsync(), count, paging);
    }
    
    public static IQueryable<T> SortAndPage<T>(this IQueryable<T> query, Paging<T> paging)
    {
        if (paging == null)
        {
            return query;
        }
        
        if (string.IsNullOrEmpty(paging.SortColumn))
        {
            paging.SortColumn = typeof(T)
                .GetProperties()
                .First(p => p.PropertyType == typeof(string)
                            || !p.PropertyType.GetInterfaces().Any(i => Collections.Any(c => i == c)))
                .Name;
        }
        
        var parameter = Expression.Parameter(typeof(T), "p");

        var command = paging.SortDirection == SortDirection.Descending ? "OrderByDescending" : "OrderBy";

        var parts = paging.SortColumn.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

        PropertyInfo property = typeof(T).GetProperty(parts[0])!;
        MemberExpression member = Expression.MakeMemberAccess(parameter, property);
        for (int i = 1; i < parts.Length; i++)
        {
            property = property.PropertyType.GetProperty(parts[i])!;
            member = Expression.MakeMemberAccess(member, property);
        }

        var orderByExpression = Expression.Lambda(member, parameter);

        Expression resultExpression = Expression.Call(
            typeof(Queryable),
            command,
            new Type[] { typeof(T), property.PropertyType },
            query.Expression,
            Expression.Quote(orderByExpression));

        foreach (var sortCriteria in paging.AdditionalSortCriteria)
        {
            command = sortCriteria.SortDirection == SortDirection.Descending ? "ThenByDescending" : "ThenBy";
            
            parameter = Expression.Parameter(typeof(T), "p");
            
            parts = sortCriteria.SortColumn.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            property = typeof(T).GetProperty(parts[0])!;
            member = Expression.MakeMemberAccess(parameter, property);
            for (int i = 1; i < parts.Length; i++)
            {
                property = property.PropertyType.GetProperty(parts[i])!;
                member = Expression.MakeMemberAccess(member, property);
            }

            orderByExpression = Expression.Lambda(member, parameter);

            resultExpression = Expression.Call(
                typeof(Queryable),
                command,
                new Type[] { typeof(T), property.PropertyType },
                resultExpression,
                Expression.Quote(orderByExpression));
        }

        query = query.Provider.CreateQuery<T>(resultExpression);

        return query.Skip(Math.Max(0, paging.Skip)).Take(paging.Top);
    }
}