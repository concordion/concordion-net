using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Command.AssertTrue
{
    [ConcordionTest]
    public class AssertTrueTest
    {
        public string successOrFailure(string fragment, string evaluationResult)
        {
            return new TestRig()
                .WithStubbedEvaluationResult(Boolean.Parse(evaluationResult))
                .ProcessFragment(fragment)
                .SuccessOrFailureInWords();
        }
    }
}
