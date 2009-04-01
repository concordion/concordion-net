using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Integration
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConcordionTestAttribute : Attribute
    {
        public string Name
        {
            get;
            private set;
        }

        public string Path
        {
            get;
            private set;
        }

        public ConcordionTestAttribute()
        {
        }
    }
}
