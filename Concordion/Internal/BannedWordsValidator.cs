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
