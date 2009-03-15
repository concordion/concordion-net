using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal.Commands
{
    public class SuccessReportedEventArgs : EventArgs
    {
        public Element Element
        {
            get;
            set;
        }
    }
}
