using Concordion.Api;
using Concordion.Api.Extension;

namespace Concordion.Spec.Concordion.Extension.JavaScript
{
    class JavaScriptLinkedExtension : IConcordionExtension
    {
        public void AddTo(IConcordionExtender concordionExtender)
        {
            concordionExtender.WithLinkedJavaScript(
                                    JavaScriptExtensionTest.SourcePath, 
                                    new global::Concordion.Api.Resource("/js/my.js"));
        }
    }
}
