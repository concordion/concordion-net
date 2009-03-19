using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Internal
{
    public static class StringExtensionMethods
    {
        public static string RemoveFirst(this string str, string toRemove)
        {
            if (String.IsNullOrEmpty(toRemove)) return String.Empty;
            var index = str.IndexOf(toRemove);
            var builder = new StringBuilder();

            if (index != -1)
            {
                builder.Append(str.Substring(0, index));
                builder.Append(str.Substring(index + toRemove.Length));
                return builder.ToString();
            }

            return str;
        }
    }
}
