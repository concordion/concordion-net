using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Extension
{
    [ConcordionTest]
    public class JavaScriptExtensionTest : AbstractExtensionTestCase
    {
        public static readonly String SourcePath = "/test/concordion/my.js";
        public static readonly String TestJs = "/* My test JS */";

        public void addLinkedJavaScriptExtension() {
            SetExtension(new JavaScriptLinkedExtension());
        }

        public void addEmbeddedJavaScriptExtension() {
            SetExtension(new JavaScriptEmbeddedExtension());
        }

        protected override void configureTestRig(TestRig testRig) {
            testRig.WithResource(new Resource(SourcePath), TestJs);
        }
    
        public bool hasJavaScriptDeclaration(string cssFilename)
        {
            return getProcessingResult().HasJavaScriptDeclaration(cssFilename);
        }

        public bool hasEmbeddedTestJavaScript() {
            return getProcessingResult().HasEmbeddedJavaScript(TestJs);
        }
    }
}
