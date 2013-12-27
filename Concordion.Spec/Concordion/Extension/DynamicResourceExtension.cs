using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Concordion.Api.Extension;
using Concordion.Api.Listener;
using Concordion.Api;

namespace Concordion.Spec.Concordion.Extension
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
            m_Target.CopyTo(new Resource("/resource/my.txt"), new StringReader("success"));
        }
    }
}
