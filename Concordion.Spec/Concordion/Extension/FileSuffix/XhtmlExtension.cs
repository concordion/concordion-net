using Concordion.Api.Extension;
using Concordion.Internal;

namespace Concordion.Spec.Concordion.Extension.FileSuffix
{
    public class XhtmlExtension : IConcordionExtension
    {
        public void AddTo(IConcordionExtender concordionExtender)
        {
            concordionExtender
                .WithSpecificationLocator(new ClassNameBasedSpecificationLocator("xhtml"));
        }
    }
}
