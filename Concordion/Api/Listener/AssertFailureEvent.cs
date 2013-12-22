using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api.Listener
{
    public class AssertFailureEvent
    {
        #region Properties

        public Element Element
        {
            get;
            private set;
        }

        public string Expected
        {
            get;
            private set;
        }

        public object Actual
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        public AssertFailureEvent(Element element, string expected, object actual) {
            this.Element = element;
            this.Expected = expected;
            this.Actual = actual;
        }

        #endregion
    }
}
