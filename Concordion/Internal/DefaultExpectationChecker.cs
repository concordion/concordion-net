using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Concordion.Internal
{
    class DefaultExpectationChecker : IExpectationChecker
    {
        #region IExpectationChecker Members

        public virtual bool IsAcceptable(string expected, object actual)
        {
            return Normalize(expected).Equals(Normalize(actual));
        }

        #endregion

        #region Methods

        public string Normalize(object obj)
        {
            string s = ConvertObjectToString(obj);
            s = ProcessLineContinuations(s);
            s = StripNewlines(s);
            s = ReplaceMultipleWhitespaceWithOneSpace(s);
            return s.Trim();
        }

        private string ReplaceMultipleWhitespaceWithOneSpace(string s)
        {
            var lineContinuationRegex = new Regex(@"[\s]+");
            var processedString = lineContinuationRegex.Replace(s, " ");

            return processedString;
        }

        private string ProcessLineContinuations(string s)
        {
            var lineContinuationRegex = new Regex(@" _");
            var processedString = lineContinuationRegex.Replace(s, String.Empty);

            return processedString;
        }

        private string StripNewlines(string s)
        {
            var newlineRegex = new Regex(@"\r?\n");
            var processedString = newlineRegex.Replace(s, String.Empty);

            return processedString;
        }

        private string ConvertObjectToString(object obj)
        {
            if (obj == null) return "(null)";
            else return obj.ToString();
        }

        #endregion
    }
}
