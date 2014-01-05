using Concordion.Integration;

namespace Concordion.Spec.Concordion.Extension.Resource
{
    [ConcordionTest]
    public class ResourceExtensionTest : AbstractExtensionTestCase
    {
        public void addResourceExtension()
        {
            this.Extension = new ResourceExtension();
        }

        public void addDynamicResourceExtension()
        {
            this.Extension = new DynamicResourceExtension();
        }

        protected override void ConfigureTestRig()
        {
            this.TestRig.WithResource(new Api.Resource(ResourceExtension.SourcePath), "0101");
        }
    
        public int getMeaningOfLife()
        {
            return 42;
        }
    }
}
