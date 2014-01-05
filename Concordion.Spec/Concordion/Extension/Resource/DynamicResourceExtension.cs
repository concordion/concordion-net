using System;
using System.IO;
using Concordion.Api;
using Concordion.Api.Extension;
using Concordion.Api.Listener;

namespace Concordion.Spec.Concordion.Extension.Resource
{
    public class DynamicResourceExtension : IConcordionExtension, IConcordionBuildListener
    {
        public static readonly String SourcePath = "/test/concordion/o.png";
        private ITarget m_Target;

        public void AddTo(IConcordionExtender concordionExtender)
        {
            concordionExtender.WithBuildListener(this);
        }

        public void ConcordionBuilt(ConcordionBuildEvent buildEvent)
        {
            this.m_Target = buildEvent.Target;
        
            this.CreateResourceInTarget();  // NOTE: normally this would be done during specification processing - eg in an AssertEqualsListener
        }

        private void CreateResourceInTarget()
        {
            this.m_Target.CopyTo(new global::Concordion.Api.Resource("/resource/my.txt"), new StringReader("success"));
        }
    }
}
