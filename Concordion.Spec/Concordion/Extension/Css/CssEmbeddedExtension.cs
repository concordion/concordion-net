using Concordion.Api.Extension;

namespace Concordion.Spec.Concordion.Extension.Css
{
    public class CssEmbeddedExtension : IConcordionExtension
    {
        public void AddTo(IConcordionExtender concordionExtender)
        {
            concordionExtender.WithEmbeddedCss(CssExtensionTest.TestCss);
        }
    }
}
