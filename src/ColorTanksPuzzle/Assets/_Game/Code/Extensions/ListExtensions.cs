using System;
using System.Collections.Generic;

namespace _Game.Code.Extensions
{
    public static class ListExtensions
    {
        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            (list[i], list[j]) = (list[j], list[i]);
        }
        
        public static void Swap<T>(this IList<T> list, T a, T b)
        {
            int i = list.IndexOf(a);
            int j = list.IndexOf(b);

            if (i < 0 || j < 0)
                throw new ArgumentException();

            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}