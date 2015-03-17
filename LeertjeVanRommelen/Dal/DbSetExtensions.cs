using System.Collections.Generic;
using System.Data.Entity;

namespace LeertjeVanRommelen.Dal
{
    internal static class DbSetExtensions
    {
        public static void AddRange<T>(this IDbSet<T> dbSet, IEnumerable<T> items) where T : class
        {
            foreach (var item in items)
            {
                dbSet.Add(item);
            }
        }
    }
}