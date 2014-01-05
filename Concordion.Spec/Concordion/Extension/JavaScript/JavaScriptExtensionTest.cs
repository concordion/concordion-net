using System;
using Concordion.Api;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Extension.JavaScript
{
    [ConcordionTest]
    public class JavaScriptExtensionTest : AbstractExtensionTestCase
    {
        public static readonly String SourcePath = "/test/concordion/my.js";
        public static readonly String TestJs = "/* My test JS */";

        public void addLinkedJavaScriptExtension()
        {
            this.Extension = new JavaScriptLinkedExtension();
        }

        public void addEmbeddedJavaScriptExtension()
        {
            this.Extension = new JavaScriptEmbeddedExtension();
        }

        protected override void ConfigureTestRig()
        {
            this.TestRig.WithResource(new global::Concordion.Api.Resource(SourcePath), TestJs);
        }
    
        public bool hasJavaScriptDeclaration(string cssFilename)
        {
            return this.ProcessingResult.HasJavaScriptDeclaration(cssFilename);
        }

        public bool hasEmbeddedTestJavaScript()
        {
            return this.ProcessingResult.HasEmbeddedJavaScript(TestJs);
        }
    }
}
