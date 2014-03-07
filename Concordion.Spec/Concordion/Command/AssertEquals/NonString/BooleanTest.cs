using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Command.AssertEquals.NonString
{
    [ConcordionTest]
    public class BooleanTest
    {
        public string OutcomeOfPerformingAssertEquals(string fragment, bool boolValue, string boolString)
        {
            return new TestRig()
                .WithStubbedEvaluationResult(boolValue)
                .ProcessFragment(Regex.Replace(fragment, "\\(some boolean string\\)", boolString))
                .SuccessOrFailureInWords();
        }
    }
}
