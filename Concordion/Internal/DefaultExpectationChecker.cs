using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Concordion.Internal
{
    public class DefaultExpectationChecker : AbstractCheckerBase
    {
        public override bool IsAcceptable(string expected, object actual)
        {
            return this.Normalize(expected).Equals(this.Normalize(actual));
        }
    }
}
