using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Spec.Concordion.Results.AssertTrue
{
    class OnSuccessTest
    {
        public bool isPalindrome(string s)
        {
            // TODO - fix this up!
            return true;//new StringBuilder(s).Reverse().ToString().Equals(s);
        }

        public string render(string fragment)
        {
            return new TestRig()
                .WithFixture(this)
                .ProcessFragment(fragment)
                .GetOutputFragmentXML();
        }
    }
}
