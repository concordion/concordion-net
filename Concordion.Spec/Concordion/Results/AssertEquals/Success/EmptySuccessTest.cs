using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Results.AssertEquals.Success
{
    [ConcordionTest]
    public class EmptySuccessTest : SuccessTest
    {
        public EmptySuccessTest()
        {
            username = String.Empty;
        }
    }
}
