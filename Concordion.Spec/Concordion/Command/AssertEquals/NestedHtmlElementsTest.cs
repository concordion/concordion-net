using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Command.AssertEquals
{
    [ConcordionTest]
    public class NestedHtmlElementsTest
    {
        public string matchOrNotMatch(string snippet, string evaluationResult)
        {
            return new TestRig()
                .WithStubbedEvaluationResult(evaluationResult)
                .ProcessFragment(snippet)
                .HasFailures ? "not match" : "match";
        }
    }
}