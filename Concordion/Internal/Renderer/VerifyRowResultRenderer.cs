using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal.Renderer
{
    public class VerifyRowResultRenderer : IVerifyRowResultListener
    {
        #region IVerifyRowResultListener Members

        public void SurplusRowFoundEventHandler(object sender, global::Concordion.Internal.Commands.SurplusRowEventArgs e)
        {
            Element element = e.RowElement;
            element.AddStyleClass("surplus");
        }

        public void MissingRowFoundEventHandler(object sender, global::Concordion.Internal.Commands.MissingRowEventArgs e)
        {
            Element element = e.RowElement;
            element.AddStyleClass("missing");
        }

        #endregion
    }
}
