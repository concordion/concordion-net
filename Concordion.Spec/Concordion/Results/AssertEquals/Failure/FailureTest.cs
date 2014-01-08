using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Integration;
using Concordion.Spec.Support;

namespace Concordion.Spec.Concordion.Results.AssertEquals.Failure
{
    [ConcordionTest]
    public class FailureTest
    {
        public string acronym;

        public string renderAsFailure(string fragment, string acronym)
        {
            this.acronym = acronym;
            return new TestRig()
                .WithFixture(this)
                .ProcessFragment(fragment)
                .GetOutputFragmentXML()
                .Replace("\u00A0", "&#160;");
        }
    }
}
