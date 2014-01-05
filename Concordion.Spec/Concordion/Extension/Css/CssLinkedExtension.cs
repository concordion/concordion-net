using Concordion.Api;
using Concordion.Api.Extension;

namespace Concordion.Spec.Concordion.Extension.Css
{
    public class CssLinkedExtension : IConcordionExtension
    {
        public void AddTo(IConcordionExtender concordionExtender)
        {
            concordionExtender.WithLinkedCss(
                                    CssExtensionTest.SourcePath, 
                                    new global::Concordion.Api.Resource("/css/my.css"));
        }
    }
}