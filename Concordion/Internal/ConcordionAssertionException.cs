using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Concordion.Internal
{
    [Serializable]
    public class ConcordionAssertionException : Exception
    {
        public ConcordionAssertionException()
            : base()
        {
        }

        public ConcordionAssertionException(string message)
            : base(message)
        {
        }

        public ConcordionAssertionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public ConcordionAssertionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
