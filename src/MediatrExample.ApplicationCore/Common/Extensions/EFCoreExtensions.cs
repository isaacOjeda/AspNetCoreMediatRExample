using System.Linq.Expressions;

namespace MediatrExample.ApplicationCore.Common.Extensions;

public static class EFCoreExtensions
{
    public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByStrValues)
        where TEntity : class
    {
        var queryExpr = source.Expression;
        var command = orderByStrValues.ToUpper().EndsWith("DESC") ? "OrderByDescending" : "OrderBy";
        var propertyName = orderByStrValues.Split(' ')[0].Trim();

        var type = typeof(TEntity);
        var property = type.GetProperties()
            .Where(item => item.Name.ToLower() == propertyName.ToLower())
            .FirstOrDefault();

        if (property == null)
            return source;

        // p
        var parameter = Expression.Parameter(type, "p");
        // p.Price
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        // p => p.Price
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);

        // Ejem. final: .OrderByDescending(p => p.Price)
        queryExpr = Expression.Call(
            type: typeof(Queryable),
            methodName: command,
            typeArguments: new Type[] { type, property.PropertyType },
            queryExpr,
            Expression.Quote(orderByExpression));

        return source.Provider.CreateQuery<TEntity>(queryExpr); ;
    }
}