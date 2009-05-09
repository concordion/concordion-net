using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Internal.Renderer;
using Concordion.Internal.Commands;
using Gallio.Model.Logging;

namespace Concordion.Integration
{
    /// <summary>
    /// Attaches the HTML specification that concordion outputs to the Gallio result
    /// </summary>
    public class GallioResultRenderer : ISpecificationListener
    {
        #region Properties
        
        /// <summary>
        /// Gets or sets the log writer.
        /// </summary>
        /// <value>The log writer.</value>
        public TestLogWriter LogWriter
        {
            get;
            private set;
        } 

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GallioResultRenderer"/> class.
        /// </summary>
        /// <param name="logWriter">The log writer.</param>
        public GallioResultRenderer(TestLogWriter logWriter)
        {
            LogWriter = logWriter;
        } 

        #endregion

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
            LogWriter.AttachXHtml(eventArgs.Resource.Name, eventArgs.Element.ToXml());
        }

        #endregion
    }
}
