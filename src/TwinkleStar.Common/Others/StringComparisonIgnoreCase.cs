using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwinkleStar.Common.Others
{
    /// <summary>
    /// 忽略大小写
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StringComparisonIgnoreCase<T> : IEqualityComparer<T>
    {
        public bool Equals(T x, T y)
        {
            if (x is String && y is String)
            {
                return x.ToString().Trim().ToLower() == y.ToString().Trim().ToLower();
            }
            return false;
        }

        public int GetHashCode(T obj)
        {
            return base.GetHashCode();
        }
    }
}
