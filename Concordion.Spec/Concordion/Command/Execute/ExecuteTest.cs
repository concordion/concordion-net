using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Spec.Concordion.Command.Execute
{
    class ExecuteTest
    {
        private bool myMethodWasCalled = false;

        public bool myMethodWasCalledProcessing(string fragment)
        {
            new TestRig()
                .WithFixture(this)
                .ProcessFragment(fragment);
            return myMethodWasCalled;
        }

        public void myMethod()
        {
            myMethodWasCalled = true;
        }
    }
}
