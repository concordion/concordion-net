using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api.Extension;

namespace Concordion.Spec.Concordion.Extension.Configuration
{
    public class FakeExtension2Factory : IConcordionExtensionFactory
    {
        public IConcordionExtension CreateExtension()
        {
            return new FakeExtension2("FakeExtension2FromFactory");
        }
    }
}
