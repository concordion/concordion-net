using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Internal.Commands;

namespace Concordion.Internal.Renderer
{
    public class SpecificationRenderer : ISpecificationRenderer
    {
        #region Fields
		        
        private static readonly string XML_DECLARATION = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

	    #endregion

        #region Properties

        private ITarget Target
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public SpecificationRenderer(ITarget target)
        {
            Target = target;
        }

        #endregion

        #region ISpecificationRenderer Members

        public void SpecificationProcessingEventHandler(object sender, SpecificationEventArgs eventArgs)
        {
        }

        public void SpecificationProcessedEventHandler(object sender, SpecificationEventArgs eventArgs)
        {
            try 
            {
                Target.Write(eventArgs.Resource, XML_DECLARATION + eventArgs.Element.ToXml());
                if (Target is FileTarget) 
                {
                    // TODO - Replace this with something meaningful
                    //Console.WriteLine(((FileTarget) target).GetFile(eventArgs.Resource).getAbsolutePath());
                }
            } 
            catch (Exception e) 
            {
                throw new Exception("Failed to write results to '" + eventArgs.Resource.Path + "'.", e);
            }
        }

        #endregion
    }
}
