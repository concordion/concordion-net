using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Spec.Concordion.Command.AssertEquals
{
    class AssertEqualsTest
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
