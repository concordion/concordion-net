using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api.Listener
{
    public class ConcordionBuildEvent
    {
        #region Properties

        public ITarget Target
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        public ConcordionBuildEvent(ITarget target)
        {
            this.Target = target;
        }

        #endregion
    }
}
