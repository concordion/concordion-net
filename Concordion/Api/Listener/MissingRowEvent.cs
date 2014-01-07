using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api.Listener
{
    public class MissingRowEvent
    {
        #region Properties

        public Element RowElement { get; private set; }

        #endregion

        #region Constructors

        public MissingRowEvent(Element rowElement)
        {
            this.RowElement = rowElement;
        }

        #endregion

    }
}
