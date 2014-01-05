using System;
using Concordion.Api;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Extension.Css
{
    [ConcordionTest]
    public class CssExtensionTest : AbstractExtensionTestCase
    {
        public static readonly string SourcePath = "/test/concordion/my.css";
        public static readonly string TestCss = "/* My test CSS */";

        public void addLinkedCSSExtension()
        {
            this.Extension = new CssLinkedExtension();
        }

        public void addEmbeddedCSSExtension()
        {
            this.Extension = new CssEmbeddedExtension();
        }

        protected override void ConfigureTestRig()
        {
            this.TestRig.WithResource(new global::Concordion.Api.Resource(SourcePath), TestCss);
        }
    
        public bool hasCSSDeclaration(String cssFilename)
        {
            return this.ProcessingResult.HasCssDeclaration(cssFilename);
        }

        public bool hasEmbeddedTestCSS()
        {
            return this.ProcessingResult.HasEmbeddedCss(TestCss);
        }
    }
}
