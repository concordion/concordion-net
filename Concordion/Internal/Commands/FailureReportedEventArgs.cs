using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal.Commands
{
    public class FailureReportedEventArgs : EventArgs
    {
        public Element Element
        {
            get;
            set;
        }

        public object Actual
        {
            get;
            set;
        }

        public string Expected
        {
            get;
            set;
        }
    }
}
