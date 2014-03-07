using System;
using System.Text.RegularExpressions;

namespace Concordion.Internal
{
    public abstract class AbstractCheckerBase : IExpectationChecker
    {
        public abstract bool IsAcceptable(string expected, object actual);

        public string Normalize(object obj)
        {
            string s = this.ConvertObjectToString(obj);
            s = this.ProcessLineContinuations(s);
            s = this.StripNewlines(s);
            s = this.ReplaceMultipleWhitespaceWithOneSpace(s);
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
    }
}