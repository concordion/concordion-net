using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Command.Run
{
    [ConcordionTest]
    public class RunTest
    {
    	public String successOrFailure(String fragment, String hardCodedTestResult, String evaluationResult) 
        {
            RunTestRunner.Result = (Result)Enum.Parse(typeof(Result), hardCodedTestResult, true);
            return new TestRig()
                .WithStubbedEvaluationResult(evaluationResult)
                .ProcessFragment(fragment)
                .SuccessOrFailureInWords();
        }
    }
}
