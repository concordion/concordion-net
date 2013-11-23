using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Concordion.Internal
{
    class BooleanExpectationChecker : DefaultExpectationChecker
    {

        public override bool IsAcceptable(string expected, object actual)
        {
            if (!(actual is bool)) return false;

            var boolActual = (bool) actual;
            var normalizedExpected = this.Normalize(expected).ToLower();
            return (boolActual && Regex.IsMatch(normalizedExpected, "true|yes|y")) ||
                   (!boolActual && Regex.IsMatch(normalizedExpected, "false|no|n|-"));
        }
    }
}
