using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Internal.Commands;

namespace Concordion.Internal.Renderer
{
    public interface IVerifyRowResultListener
    {
        void SurplusRowFoundEventHandler(object sender, SurplusRowEventArgs e);
        void MissingRowFoundEventHandler(object sender, MissingRowEventArgs e);
    }
}
