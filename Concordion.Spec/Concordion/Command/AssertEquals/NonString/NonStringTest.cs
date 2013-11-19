using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Command.AssertEquals.NonString
{
    [ConcordionTest]
    public class NonStringTest
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
                var customCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                customCulture.NumberFormat.NumberDecimalSeparator = ".";
                System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

                simulatedResult = Double.Parse(result);
            }
            else
            {
                throw new Exception("Unsupported result-type '" + resultType + "'.");
            }

            fragment = Regex.Replace(fragment, "\\(some expectation\\)", expectedString);

            return new TestRig()
                .WithStubbedEvaluationResult(simulatedResult)
                .ProcessFragment(fragment)
                .SuccessOrFailureInWords();
        }
    }
}
