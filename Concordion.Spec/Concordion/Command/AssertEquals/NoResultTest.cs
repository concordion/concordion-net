using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Spec.Concordion.Command.AssertEquals
{
    class NoResultTest
    {
        public string outcomeOfPerformingAssertEquals(string snippet, string expectedString, string result)
        {
            if (result.Equals("null"))
            {
                result = null;
            }
            snippet = snippet.Replace("\\(some expectation\\)", expectedString);

            return new TestRig()
                .WithStubbedEvaluationResult(result)
                .ProcessFragment(snippet)
                .SuccessOrFailureInWords();
        }
    }
}
