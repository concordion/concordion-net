using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal
{
    class ExpectedToPassFixtureState : IFixtureState
    {
        #region IFixtureState Members

        public void AssertIsSatisfied(long successCount, long failureCount, long exceptionCount)
        {
            if (failureCount > 0L)
            {
                throw new AssertionErrorException("Specification has failure(s). See output HTML for details.");
            }

            if (exceptionCount > 0L)
            {
                throw new AssertionErrorException("Specification has exception(s). See output HTML for details.");
            }
        }

        #endregion
    }
}
