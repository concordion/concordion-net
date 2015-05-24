using System;
using System.Collections.Generic;
using System.Linq;
using Concordion.Api.Extension;

namespace Concordion.Spec.Concordion.Extension.Configuration
{
    [Extensions(typeof(FakeExtension1))]
    public class ExampleFixtureWithCombinedConfiguration : FakeExtensionBase
    {
        [Extension]
        public FakeExtension2 extension2 = new FakeExtension2();
    }
}
