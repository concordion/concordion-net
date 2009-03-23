using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Spec.Concordion.Command.Set
{
    class SetTest
    {
        private string param;

        public void process(string fragment)
        {
            new TestRig()
                .WithFixture(this)
                .ProcessFragment(fragment);
        }

        public string getParameterPassedIn()
        {
            return param;
        }

        public void setUpUser(string fullName)
        {
            this.param = fullName;
        }
    }
}
