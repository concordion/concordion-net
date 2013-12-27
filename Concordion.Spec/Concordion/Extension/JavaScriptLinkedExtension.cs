using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Api.Extension;

namespace Concordion.Spec.Concordion.Extension
{
    class JavaScriptLinkedExtension : IConcordionExtension
    {
        public void AddTo(IConcordionExtender concordionExtender)
        {
            concordionExtender.WithLinkedJavaScript(JavaScriptExtensionTest.SourcePath, new Resource("/js/my.js"));
        }
    }
}
