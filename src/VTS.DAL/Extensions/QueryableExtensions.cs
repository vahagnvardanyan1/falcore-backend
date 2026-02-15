using System.Linq.Expressions;

namespace VTS.DAL.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
    {
        if (condition)
        {
            source = source.Where(predicate);
        }

        return source;
    }
}
