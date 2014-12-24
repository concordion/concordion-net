using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Integration;
using Concordion.Spec.Support;

namespace Concordion.Spec.Concordion.Command.AssertEquals
{
    [ConcordionTest]
    public class CaseSensitiveTest
    {
        public string successOrFailure(string fragment, string evaluationResult)
        {
            return new TestRig()
                .WithStubbedEvaluationResult(evaluationResult)
                .ProcessFragment(fragment)
                .SuccessOrFailureInWords();
        }
    }
}
