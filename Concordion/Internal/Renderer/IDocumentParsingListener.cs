using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Internal.Renderer
{
    public interface IDocumentParsingListener
    {
        void DocumentParsingEventHandler(object sender, DocumentParsingEventArgs e);
    }
}
