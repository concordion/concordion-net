using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Spec.Concordion.Command.Echo
{
    class EscapingHtmlCharactersTest
    {
        public string render(string fragment, string evalResult)
        {
            return new TestRig()
                .WithStubbedEvaluationResult(evalResult)
                .ProcessFragment(fragment)
                .GetOutputFragmentXML()
                .Replace(" concordion:echo=\"username\">", ">");
        }
    }
}
