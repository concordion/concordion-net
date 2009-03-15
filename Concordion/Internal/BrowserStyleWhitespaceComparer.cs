using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Concordion.Internal
{
    public class BrowserStyleWhitespaceComparer : IComparer<object>
    {
        #region Methods

        private static string Normalize(object obj)
        {
            string s = ConvertObjectToString(obj);
            s = ProcessLineContinuations(s);
            s = ReplaceMultipleWhitespaceWithOneSpace(s);
            return s.Trim();
        }

        private static string ReplaceMultipleWhitespaceWithOneSpace(string s)
        {
            var lineContinuationRegex = new Regex(@"[\s\n\r]+");
            var processedString = lineContinuationRegex.Replace(s, " ");

            return processedString;
        }

        private static string ProcessLineContinuations(string s)
        {
            var lineContinuationRegex = new Regex(@" _[\n\r+]");
            var processedString = lineContinuationRegex.Replace(s, "");

            return processedString;
        }

        private static string ConvertObjectToString(object obj)
        {
            if (obj == null) return "(null)";
            else return obj.ToString();
        }

        #endregion

        #region IComparer<object> Members

        public int Compare(object x, object y)
        {
            return Normalize(x).CompareTo(Normalize(y));
        }

        #endregion
    }
}
