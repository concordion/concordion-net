using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Spec.Concordion.Command.AssertEquals
{
    class NonStringTest
    {
        public string outcomeOfPerformingAssertEquals(string fragment, string expectedString, string result, string resultType)
        {
            object simulatedResult;
            if (resultType.Equals("String"))
            {
                simulatedResult = result;
            }
            else if (resultType.Equals("Integer"))
            {
                simulatedResult = Int32.Parse(result);
            }
            else if (resultType.Equals("Double"))
            {
                simulatedResult = Double.Parse(result);
            }
            else
            {
                throw new Exception("Unsupported result-type '" + resultType + "'.");
            }

            fragment = fragment.Replace("\\(some expectation\\)", expectedString);

            return new TestRig()
                .WithStubbedEvaluationResult(simulatedResult)
                .ProcessFragment(fragment)
                .SuccessOrFailureInWords();
        }
    }
}
