using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api.Extension;
using Concordion.Internal;

namespace Concordion.Spec.Concordion.Extension
{
    public class XhtmlExtension : IConcordionExtension
    {
        public void AddTo(IConcordionExtender concordionExtender)
        {
            concordionExtender
                .WithSpecificationLocator(new ClassNameBasedSpecificationLocator("xhtml"))
                .WithTarget(new FileTargetWithSuffix("html"));
        }
    }
}
