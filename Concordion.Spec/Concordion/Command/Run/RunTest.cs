using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Spec.Concordion.Command.Run
{
    class RunTest
    {
    	public String successOrFailure(String fragment, String hardCodedTestResult, String evaluationResult) 
        {
            RunTestRunner.result = (Result)Enum.Parse(typeof(Result), hardCodedTestResult, true);
            return new TestRig()
                .WithStubbedEvaluationResult(evaluationResult)
                .ProcessFragment(fragment)
                .SuccessOrFailureInWords();
        }
    }
}
