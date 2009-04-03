using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Reflection;

namespace Gallio.ConcordionAdapter.Model
{
    public class ConcordionTypeInfoAdapter
    {
        public ITypeInfo Target
        {
            get;
            private set;
        }

        public ConcordionTypeInfoAdapter(ITypeInfo target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            Target = target;
        }
    }
}
