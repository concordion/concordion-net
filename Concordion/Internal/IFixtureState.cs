using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Internal
{
    interface IFixtureState
    {
        void AssertIsSatisfied(long successCount, long failureCount, long exceptionCount);
    }
}
