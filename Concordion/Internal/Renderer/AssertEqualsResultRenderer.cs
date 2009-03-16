using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal.Renderer
{
    public class AssertEqualsResultRenderer : IAssertEqualsListener
    {
        #region IAssertEqualsListener Members

        public void SuccessReportedEventHandler(object sender, global::Concordion.Internal.Commands.SuccessReportedEventArgs e)
        {
            e.Element
                .AddStyleClass("success")
                .AppendNonBreakingSpaceIfBlank();
        }

        public void FailureReportedEventHandler(object sender, global::Concordion.Internal.Commands.FailureReportedEventArgs e)
        {
            Element element = e.Element;
            element.AddStyleClass("failure");
            
            Element spanExpected = new Element("del");
            spanExpected.AddStyleClass("expected");
            element.MoveChildrenTo(spanExpected);
            element.AppendChild(spanExpected);
            spanExpected.AppendNonBreakingSpaceIfBlank();
            
            Element spanActual = new Element("ins");
            spanActual.AddStyleClass("actual");
            spanActual.AppendText(e.Actual.ToString());
            spanActual.AppendNonBreakingSpaceIfBlank();
            
            element.AppendText("\n");
            element.AppendChild(spanActual);   
        }

        #endregion
    }
}
