using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api.Listener
{
    public class ExceptionCaughtEvent
    {
        #region Properties

        public Exception CaughtException
        {
            get;
            private set;
        }

        public Element Element
        {
            get;
            private set;
        }

        public string Expression
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        public ExceptionCaughtEvent(Exception exception, Element element, string expression)
        {
            this.CaughtException = exception;
            this.Element = element;
            this.Expression = expression;
        }

        #endregion
    }
}
