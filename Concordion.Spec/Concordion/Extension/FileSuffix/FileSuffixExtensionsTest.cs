using Concordion.Api.Extension;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Extension.FileSuffix
{
    [ConcordionTest]
    [Extensions(typeof(XhtmlExtension))]
    public class FileSuffixExtensionsTest
    {
        public bool hasBeenProcessed()
        {
            return true;
        }
    }
}
