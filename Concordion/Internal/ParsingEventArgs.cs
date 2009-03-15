using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Concordion.Internal
{
    public class DocumentParsingEventArgs : EventArgs
    {
        public XDocument Document
        {
            get;
            set;
        }
    }
}
