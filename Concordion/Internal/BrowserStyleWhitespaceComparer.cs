// Copyright 2009 Jeffrey Cameron
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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

        public static string Normalize(object obj)
        {
            string s = ConvertObjectToString(obj);
            s = ProcessLineContinuations(s);
            s = StripNewlines(s);
            s = ReplaceMultipleWhitespaceWithOneSpace(s);
            return s.Trim();
        }

        private static string ReplaceMultipleWhitespaceWithOneSpace(string s)
        {
            var lineContinuationRegex = new Regex(@"[\s]+");
            var processedString = lineContinuationRegex.Replace(s, " ");

            return processedString;
        }

        private static string ProcessLineContinuations(string s)
        {
            var lineContinuationRegex = new Regex(@" _");
            var processedString = lineContinuationRegex.Replace(s, String.Empty);

            return processedString;
        }

        private static string StripNewlines(string s)
        {
            var newlineRegex = new Regex(@"\r?\n");
            var processedString = newlineRegex.Replace(s, String.Empty);

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
