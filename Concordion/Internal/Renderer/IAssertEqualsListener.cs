using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Internal.Commands;

namespace Concordion.Internal.Renderer
{
    public interface IAssertEqualsListener
    {
        void SuccessReportedEventHandler(object sender, SuccessReportedEventArgs e);
        void FailureReportedEventHandler(object sender, FailureReportedEventArgs e);
    }
}
