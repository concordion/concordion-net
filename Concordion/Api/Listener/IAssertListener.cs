using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api.Listener
{
    public interface IAssertListener
    {
        void SuccessReported(AssertSuccessEvent successEvent);
    
        void FailureReported(AssertFailureEvent failureEvent);
    }
}
