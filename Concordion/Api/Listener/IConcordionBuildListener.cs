using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api.Listener
{
    public interface IConcordionBuildListener
    {
        void ConcordionBuilt(ConcordionBuildEvent buildEvent);
    }
}
