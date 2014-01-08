using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api.Listener
{
    public class RunFailureEvent
    {
        #region Properties

        public Element Element { get; private set; }

        #endregion

        #region Constructors

        public RunFailureEvent(Element element)
        {
            this.Element = element;
        }

        #endregion

    }
}
