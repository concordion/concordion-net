using Concordion.Api.Extension;

namespace Concordion.Spec.Concordion.Extension.JavaScript
{
    public class JavaScriptEmbeddedExtension : IConcordionExtension
    {
        public void AddTo(IConcordionExtender concordionExtender)
        {
            concordionExtender.WithEmbeddedJavaScript(JavaScriptExtensionTest.TestJs);
        }
    }
}
