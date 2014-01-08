using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Integration;
using Concordion.Spec.Support;

namespace Concordion.Spec.Concordion.Command.Execute
{
    [ConcordionTest]
    public class ExecutingTablesTest
    {
        public Result process(string fragment)
        {
            ProcessingResult r = new TestRig()
                .WithFixture(this)
                .ProcessFragment(fragment);
            
            Result result = new Result();
            result.successCount = r.SuccessCount;
            result.failureCount = r.FailureCount;
            result.exceptionCount = r.ExceptionCount;
            
            // TODO - repair this
            var lastEvent = r.GetLastAssertEqualsFailureEvent();
            if (lastEvent != null)
            {
                result.lastActualValue = lastEvent.Actual;
                result.lastExpectedValue = lastEvent.Expected;
            }
            
            return result;
        }

        public string generateUsername(string fullName) 
        {
            return fullName.Replace(" ", "").ToLower();
        }

        public struct Result 
        {
            public long successCount;
            public long failureCount;
            public long exceptionCount;
            public string lastExpectedValue;
            public object lastActualValue;
        }
    }
}
