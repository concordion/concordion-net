using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Extension
{
    [ConcordionTest]
    public class ResourceExtensionTest : AbstractExtensionTestCase
    {
        public void addResourceExtension() {
            SetExtension(new ResourceExtension());
        }

        public void addDynamicResourceExtension() {
            SetExtension(new DynamicResourceExtension());
        }

        protected override void configureTestRig(TestRig testRig) {
            testRig.WithResource(new Resource(ResourceExtension.SourcePath), "0101");
        }
    
        public int getMeaningOfLife() {
            return 42;
        }
    }
}
