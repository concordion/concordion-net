using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api.Listener;
using Concordion.Internal.Renderer;
using Concordion.Internal.Commands;
using Gallio.Framework;

namespace Concordion.Integration
{
    /// <summary>
    /// Attaches the HTML specification that concordion outputs to the Gallio result
    /// </summary>
    public class GallioResultRenderer : ISpecificationProcessingListener
    {
        #region ISpecificationListener Members

        /// <summary>
        /// Handles the SpecificationProcessing event that is triggered before the specification is processed
        /// </summary>
        public void BeforeProcessingSpecification(SpecificationProcessingEvent processingEvent)
        {
        }

        /// <summary>
        /// Handles the SpecificationProcessed event that is triggered after the specification is processed
        /// </summary>
        public void AfterProcessingSpecification(SpecificationProcessingEvent processingEvent)
        {
            TestLog.AttachXHtml(processingEvent.Resource.Name, processingEvent.RootElement.ToXml());
        }

        #endregion
    }
}
