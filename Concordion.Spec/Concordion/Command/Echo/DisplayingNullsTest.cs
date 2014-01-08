using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Internal;
using Concordion.Integration;
using Concordion.Spec.Support;

namespace Concordion.Spec.Concordion.Command.Echo
{
    [ConcordionTest]
    public class DisplayingNullsTest
    {
        public string render(string fragment)
        {
            return new TestRig()
                .WithStubbedEvaluationResult(null)
                .ProcessFragment(fragment)
                .GetOutputFragmentXML()
                .Replace(" concordion:echo=\"username\">", ">");
        }
    }
}
