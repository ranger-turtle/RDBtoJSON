using System;
using System.Collections.Generic;

namespace RDBtoJSON
{
    public static class JontekExtensions
    {
        public static void OnEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (action == null)
                throw new ArgumentNullException("action");

            foreach (var item in source)
            {
                action.Invoke(item);
            }
        }
    }
}
