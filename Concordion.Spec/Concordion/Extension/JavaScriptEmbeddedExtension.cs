using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api.Extension;

namespace Concordion.Spec.Concordion.Extension
{
    public class JavaScriptEmbeddedExtension : IConcordionExtension
    {
        public void AddTo(IConcordionExtender concordionExtender)
        {
            concordionExtender.WithEmbeddedJavaScript(JavaScriptExtensionTest.TestJs);
        }
    }
}
