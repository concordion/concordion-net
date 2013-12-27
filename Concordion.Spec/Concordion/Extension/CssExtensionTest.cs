using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Extension
{
    [ConcordionTest]
    public class CssExtensionTest : AbstractExtensionTestCase
    {
        public static readonly string SourcePath = "/test/concordion/my.css";
        public static readonly string TestCss = "/* My test CSS */";

        public void addLinkedCSSExtension() {
            SetExtension(new CssLinkedExtension());
        }

        public void addEmbeddedCSSExtension() {
            SetExtension(new CssEmbeddedExtension());
        }

        protected override void configureTestRig(TestRig testRig) {
            testRig.WithResource(new Resource(SourcePath), TestCss);
        }
    
        public bool hasCSSDeclaration(String cssFilename) {
            return getProcessingResult().HasCssDeclaration(cssFilename);
        }

        public bool hasEmbeddedTestCSS() {
            return getProcessingResult().HasEmbeddedCss(TestCss);
        }
    }
}
