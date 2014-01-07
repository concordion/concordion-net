using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api.Listener
{
    public class SurplusRowEvent
    {
        #region Properties

        public Element RowElement { get; private set; }

        #endregion

        #region Constructors

        public SurplusRowEvent(Element rowElement)
        {
            this.RowElement = rowElement;
        }

        #endregion

    }
}
