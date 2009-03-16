using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Internal.Commands;

namespace Concordion.Internal.Renderer
{
    public class RunResultRenderer : IAssertEqualsListener
    {
        #region Methods

        public void IgnoredReportedEventHandler(object sender, FailureReportedEventArgs e)
        {
            e.Element.AddStyleClass("ignored").AppendNonBreakingSpaceIfBlank();
        }

        #endregion

        #region IAssertEqualsListener Members

        public void SuccessReportedEventHandler(object sender, SuccessReportedEventArgs e)
        {
            e.Element.AddStyleClass("success").AppendNonBreakingSpaceIfBlank();
        }

        public void FailureReportedEventHandler(object sender, FailureReportedEventArgs e)
        {
            e.Element.AddStyleClass("failure").AppendNonBreakingSpaceIfBlank();
        }

        #endregion
    }
}
