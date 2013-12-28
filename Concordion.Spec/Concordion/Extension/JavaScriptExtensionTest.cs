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

        public void addLinkedJavaScriptExtension()
        {
            Extension = new JavaScriptLinkedExtension();
        }

        public void addEmbeddedJavaScriptExtension()
        {
            Extension = new JavaScriptEmbeddedExtension();
        }

        protected override void ConfigureTestRig()
        {
            TestRig.WithResource(new Resource(SourcePath), TestJs);
        }
    
        public bool hasJavaScriptDeclaration(string cssFilename)
        {
            return ProcessingResult.HasJavaScriptDeclaration(cssFilename);
        }

        public bool hasEmbeddedTestJavaScript()
        {
            return ProcessingResult.HasEmbeddedJavaScript(TestJs);
        }
    }
}
