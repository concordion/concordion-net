using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Internal.Commands
{
    public interface IResultReporter
    {
        event EventHandler<SuccessReportedEventArgs> SuccessReported;
        event EventHandler<FailureReportedEventArgs> FailureReported;
    }
}
