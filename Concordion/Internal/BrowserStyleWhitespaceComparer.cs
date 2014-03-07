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
using Concordion.Internal.Util;

namespace Concordion.Internal
{
    public class BrowserStyleWhitespaceComparer : IComparer<object>
    {
        private readonly ChainOfExpectationCheckers m_ChainOfCheckers = new ChainOfExpectationCheckers();

        public BrowserStyleWhitespaceComparer()
        {
            this.m_ChainOfCheckers.Add(new DefaultExpectationChecker());
            this.m_ChainOfCheckers.Add(new BooleanExpectationChecker());
        }

        #region IComparer<object> Members

        public int Compare(object x, object y)
        {
            Check.IsTrue(y is string, "This comparator only supports comparisons with String objects");
            if (m_ChainOfCheckers.IsAcceptable((string) y, x))
            {
                return 0;
            }
            return -1;
        }

        #endregion
    }
}
