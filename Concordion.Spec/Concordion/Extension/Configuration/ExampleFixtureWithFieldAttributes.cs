using System;
using System.Collections.Generic;
using System.Linq;
using Concordion.Api.Extension;

namespace Concordion.Spec.Concordion.Extension.Configuration
{
    public class ExampleFixtureWithFieldAttributes
    {
        [Extension]
        public IConcordionExtension extension = new FakeExtension1();

        [Extension]
        public FakeExtension2 extension2 = new FakeExtension2();
    }
}
