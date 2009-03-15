using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Internal.Util
{
    public class Check
    {
        public static void IsTrue(bool expression, string message, params object[] args)
        {
            if (!expression)
            {
                throw new Exception(String.Format(message, args));
            }
        }

        public static void IsFalse(bool expression, string message, params object[] args)
        {
            IsTrue(!expression, message, args);
        }

        public static void NotNull(object obj, string message, params object[] args)
        {
            IsTrue(obj != null, message, args);
        }

        public static void NotEmpty(string str, string message, params object[] args)
        {
            IsTrue(str == string.Empty, message, args);
        }
    }
}
