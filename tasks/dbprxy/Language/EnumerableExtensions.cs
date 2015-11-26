using System;
using System.Collections.Generic;
using System.Linq;

namespace Language
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> items, Random random = null)
        {
            random = random ?? new Random();
            var copy = items.ToList();
            for (int i = 0; i < copy.Count; i++)
            {
                var nextIndex = random.Next(i, copy.Count);
                yield return copy[nextIndex];
                copy[nextIndex] = copy[i];
            }
        }
    }
}