using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api.Listener
{
    public interface IRunListener : IExceptionCaughtListener
    {
        void SuccessReported(RunSuccessEvent runSuccessEvent);

        void FailureReported(RunFailureEvent runFailureEvent);

        void IgnoredReported(RunIgnoreEvent runIgnoreEvent);
    }
}
