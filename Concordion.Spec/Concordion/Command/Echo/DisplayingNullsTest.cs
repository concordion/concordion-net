using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Internal;

namespace Concordion.Spec.Concordion.Command.Echo
{
    class DisplayingNullsTest
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
