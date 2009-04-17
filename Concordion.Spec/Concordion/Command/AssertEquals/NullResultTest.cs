using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Integration;
using System.Text.RegularExpressions;

namespace Concordion.Spec.Concordion.Command.AssertEquals.NonString
{
    [ConcordionTest]
    public class NullResultTest
    {
        public string outcomeOfPerformingAssertEquals(string snippet, string expectedString, string result)
        {
            if (result.Equals("null"))
            {
                result = null;
            }

            snippet = Regex.Replace(snippet, "\\(some expectation\\)", expectedString);

            return new TestRig()
                .WithStubbedEvaluationResult(result)
                .ProcessFragment(snippet)
                .SuccessOrFailureInWords();
        }
    }
}
