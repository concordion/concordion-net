using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Results.AssertEquals.Success
{
    [ConcordionTest]
    public class SuccessTest
    {
        public string username = "fred";
    
        public string renderAsSuccess(string fragment)
        {
            return new TestRig()
                .WithFixture(this)
                .ProcessFragment(fragment)
                .GetOutputFragmentXML();
        }
    }
}
