using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Model.Helpers;
using System.IO;

namespace Gallio.ConcordionAdapter.Model
{
    internal class ConcordionTestDriver : SimpleTestDriver
    {
        
        protected override string FrameworkName
        {
            get { return "Concordion"; }
        }

        protected override TestExplorer CreateTestExplorer()
        {
            return new ConcordionTestExplorer();
        }

        protected override TestController CreateTestController()
        {
            return new DelegatingTestController(test =>
            {
                var topTest = test as ConcordionTest;
                return topTest != null ? new ConcordionTestController() : null;
            });
        }
    }
}
