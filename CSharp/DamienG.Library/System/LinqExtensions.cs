using System;
using System.Collections.Generic;
using System.Linq;

namespace DamienG.System
{
    public static class LinqExtensions
    {
        private static readonly Random random = new Random();

        public static IEnumerable<T> OrderBySequence<T, TProperty>(this IEnumerable<T> source,
                                                                   Func<T, TProperty> property, IEnumerable<TProperty> sequence)
        {
            var sequenceList = sequence.ToList();
            var sequenceDictionary = sequenceList.ToDictionary(s => s, sequenceList.IndexOf);
            return source.OrderBy(s => sequenceDictionary[property(s)]);
        }

        public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int page, int pageSize)
        {
            if (page < 1 || pageSize < 1)
                throw new ArgumentException("Must be 1 or greater", page < 1 ? "page" : "pageSize");

            return source.Skip(--page*pageSize).Take(pageSize);
        }

        public static T ContainsOrDefault<T>(this IEnumerable<T> source, T value)
        {
            return source.Contains(value) ? value : default(T);
        }

        public static T RandomElement<T>(this ICollection<T> collection)
        {
            return collection.ElementAt(random.Next(collection.Count));
        }

        public static TResult FirstOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
        {
            var match = source.FirstOrDefault(predicate);
            return Equals(match, default(TSource)) ? default(TResult) : selector(match);
        }
    }
}