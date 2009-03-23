using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Spec.Concordion.Command.AssertFalse
{
    class AssertFalseTest
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
