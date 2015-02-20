using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Core;

namespace Concordion.NUnit.Addin
{
    public class ConcordionTestFixture : TestFixture
    {
        public ConcordionTestFixture(Type fixtureType)
            : base(fixtureType) { }

        public override TestResult Run(EventListener listener, ITestFilter filter)
        {
            return base.Run(listener, TestFilter.Empty);
        }

        protected override void DoOneTimeSetUp(TestResult suiteResult) { }

        protected override void DoOneTimeTearDown(TestResult suiteResult) { }
    }
}
