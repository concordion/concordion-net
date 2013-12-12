using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Internal
{
    public class ChainOfExpectationCheckers : IExpectationChecker
    {
        private List<IExpectationChecker> checkers = new List<IExpectationChecker>();

        public ChainOfExpectationCheckers Add(IExpectationChecker checker)
        {
            checkers.Add(checker);
            return this;
        }

        public bool IsAcceptable(string expected, object actual)
        {
            return this.checkers.Any(checker => checker.IsAcceptable(expected, actual));
        }
    }
}
