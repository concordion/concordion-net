using System;
using System.Collections.Generic;
using System.Linq;
using Concordion.Api.Extension;

namespace Concordion.Spec.Concordion.Extension.Configuration
{
    class ExampleDerivedFixtureWithFieldAttributes : ExampleFixtureBaseWithFieldAttributes
    {
        [Extension]
        public IConcordionExtension extension = new FakeExtension1("ExampleExtension");
    }
}
