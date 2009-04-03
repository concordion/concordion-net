using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Internal.Renderer;
using Gallio.Model.Logging;

namespace Concordion.Integration
{
    public class GallioResultRenderer : ISpecificationListener
    {
        public TestLogWriter LogWriter
        {
            get;
            private set;
        }

        public GallioResultRenderer(TestLogWriter logWriter)
        {
            LogWriter = logWriter;
        }



        #region ISpecificationListener Members

        public void SpecificationProcessingEventHandler(object sender, global::Concordion.Internal.Commands.SpecificationEventArgs eventArgs)
        {
        }

        public void SpecificationProcessedEventHandler(object sender, global::Concordion.Internal.Commands.SpecificationEventArgs eventArgs)
        {
            LogWriter.AttachXHtml(eventArgs.Resource.Name, eventArgs.Element.ToXml());
        }

        #endregion
    }
}
