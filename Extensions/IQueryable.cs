using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Extensions
{
    /// <summary>
    /// Linq to SQL.
    /// </summary>
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> query, int pageSize, int page)
        {
            return query.Skip(page * pageSize).Take(pageSize);
        }
        public static Task<List<T>> ToListAsync<T>(this IQueryable<T> list)
        {
            return Task.Run(() => list.ToList());
        }
        public static string ToSQLQuery<T>(this IQueryable<T> source)
        {
            var x = string.IsNullOrEmpty(Convert.ToString(source)) ? "" : source.ToString().Replace("[Extent", "[D");
            return x;
        }
    }
}
