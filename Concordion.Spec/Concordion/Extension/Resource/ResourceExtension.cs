using System;
using Concordion.Api.Extension;

namespace Concordion.Spec.Concordion.Extension.Resource
{
    public class ResourceExtension : IConcordionExtension
    {
        public static readonly String SourcePath = "/test/concordion/o.png";

        public void AddTo(IConcordionExtender concordionExtender)
        {
            concordionExtender.WithResource(
                                    SourcePath, 
                                    new global::Concordion.Api.Resource(("/images/o.png")));
        }
    }
}
