using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Spec.Concordion.Command.AssertEquals
{
    class ExceptionsTest
    {
        public object countsFromExecutingSnippetWithSimulatedEvaluationResult(string snippet, string simulatedResult)
        {
            TestRig harness = new TestRig();
            if (simulatedResult.Equals("(An exception)"))
            {
                harness.WithStubbedEvaluationResult(new Exception("simulated exception"));
            }
            else
            {
                harness.WithStubbedEvaluationResult(simulatedResult);
            }
            return harness.ProcessFragment(snippet);
        }
    }
}
