using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Concordion.Api.Listener
{
    public interface IDocumentParsingListener
    {
        void BeforeParsing(XDocument document);
    }
}
