using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Internal.Renderer;
using Concordion.Internal.Commands;
using Gallio.Framework;

namespace Concordion.Integration
{
    /// <summary>
    /// Attaches the HTML specification that concordion outputs to the Gallio result
    /// </summary>
    public class GallioResultRenderer : ISpecificationListener
    {
        #region ISpecificationListener Members

        /// <summary>
        /// Handles the SpecificationProcessing event that is triggered before the specification is processed
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="SpecificationEventArgs"/> instance containing the event data.</param>
        public void SpecificationProcessingEventHandler(object sender, SpecificationEventArgs eventArgs)
        {
        }

        /// <summary>
        /// Handles the SpecificationProcessed event that is triggered after the specification is processed
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="SpecificationEventArgs"/> instance containing the event data.</param>
        public void SpecificationProcessedEventHandler(object sender, SpecificationEventArgs eventArgs)
        {
            TestLog.AttachXHtml(eventArgs.Resource.Name, eventArgs.Element.ToXml());
        }

        #endregion
    }
}
