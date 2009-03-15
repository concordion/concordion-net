using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal.Commands
{
    public class ExceptionCaughtEventArgs : EventArgs
    {
        public Element Element
        {
            get;
            set;
        }

        public Exception Exception
        {
            get;
            set;
        }

        public string Expression
        {
            get;
            set;
        }
    }
}
