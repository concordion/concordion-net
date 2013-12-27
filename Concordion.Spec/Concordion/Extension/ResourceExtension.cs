using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Api.Extension;

namespace Concordion.Spec.Concordion.Extension
{
    public class ResourceExtension : IConcordionExtension
    {
        public static readonly String SourcePath = "/test/concordion/o.png";

        public void AddTo(IConcordionExtender concordionExtender)
        {
            concordionExtender.WithResource(SourcePath, new Resource(("/images/o.png")));
        }
    }
}
