using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api.Listener
{
    public class SpecificationProcessingEvent
    {
        #region Properties

        public Element RootElement
        {
            get;
            private set;
        }

        public Resource Resource
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        public SpecificationProcessingEvent(Resource resource, Element rootElement)
        {
            this.Resource = resource;
            this.RootElement = rootElement;
        }

        #endregion
    }
}
