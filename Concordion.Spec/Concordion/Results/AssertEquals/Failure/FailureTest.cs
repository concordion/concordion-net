using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Spec.Concordion.Results.AssertEquals.Failure
{
    class FailureTest
    {
        public string acronym;

        public string renderAsFailure(string fragment, string acronym)
        {
            this.acronym = acronym;
            return new TestRig()
                .WithFixture(this)
                .ProcessFragment(fragment)
                .GetOutputFragmentXML();
        }
    }
}
