using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Integration;
using NUnit.Framework;

namespace Concordion.Spec.Concordion.Command
{
    [ConcordionTest]
    public class EvaluatingCommandsTest
    {
        public static string assertEqualsResult = "assertequals";

        public static bool assertTrueResult = true;

        public static bool assertFalseResult = false;

        public static Exception exceptionResult;

        public static IList<string> verifyRowsResult = new[] { "value1", "value2" };

        public string ForAssertEquals()
        {
            return assertEqualsResult;
        }

        public bool ForAssertTrue()
        {
            return assertTrueResult;
        }

        public bool ForAssertFalse()
        {
            return assertFalseResult;
        }

        public void ForException()
        {
            if (exceptionResult != null)
            {
                throw exceptionResult;
            }
        }

        public IEnumerable ForVerifyRows()
        {
            return verifyRowsResult;
        }
    }
}
