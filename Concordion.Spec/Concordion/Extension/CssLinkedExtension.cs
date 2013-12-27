using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Api.Extension;

namespace Concordion.Spec.Concordion.Extension
{
    public class CssLinkedExtension : IConcordionExtension
    {
        public void AddTo(IConcordionExtender concordionExtender)
        {
            concordionExtender.WithLinkedCss(CssExtensionTest.SourcePath, new Resource("/css/my.css"));
        }
    }
}