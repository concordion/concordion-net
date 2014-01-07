using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api.Listener
{
    public class ExpressionEvaluatedEvent
    {
        #region Properties

        public Element Element { get; private set; }

        #endregion

        #region Constructors

        public ExpressionEvaluatedEvent(Element rowElement)
        {
            this.Element = rowElement;
        }

        #endregion

    }
}
