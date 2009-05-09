using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal
{
    class ExpectedToFailFixtureState : IFixtureState
    {
        #region IFixtureState Members

        public void AssertIsSatisfied(long successCount, long failureCount, long exceptionCount)
        {
            if (failureCount + exceptionCount == 0)
            {
                throw new AssertionErrorException("Specification is expected to fail but has neither failures nor exceptions.");
            }
        }

        #endregion
    }
}
