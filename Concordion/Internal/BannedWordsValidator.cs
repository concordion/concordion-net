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

namespace Concordion.Internal
{
    public class BannedWordsValidator : IExpressionValidator
    {
        #region Properties

        private static readonly List<string> BannedWords = new List<string> { "click", "doubleClick", "enter", "open", "press", "type" };

        #endregion

        #region Constructors

        public BannedWordsValidator()
        {
        }

        #endregion

        #region IExpressionValidator Members

        public void Validate(string expression)
        {
            foreach (string bannedWord in BannedWords)
            {
                if (expression.StartsWith(bannedWord))
                {
                    throw new InvalidOperationException("Expression starts with a banned word ('" + bannedWord + "').\n"
                        + "This word strongly suggests you are writing a script.\n"
                        + "Concordion is a specification tool not a scripting tool.\n"
                        + "See the website http://www.concordion.org for more information.");
                }
            }
        }

        #endregion
    }
}
