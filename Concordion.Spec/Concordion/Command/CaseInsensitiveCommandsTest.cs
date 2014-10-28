using System;
using System.Collections.Generic;
using System.Linq;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Command
{
    [ConcordionTest]
    public class CaseInsensitiveCommands
    {
        public string process(string snippet, object stubbedResult)
        {
            long successCount = new TestRig()
                .WithStubbedEvaluationResult(stubbedResult)
                .ProcessFragment(snippet)
                .SuccessCount;
            
            return successCount == 1 ? snippet : "Did not work";
        }
    }
}
