using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Spec.Concordion.Command.AssertEquals
{
    class NestedElementsTest
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