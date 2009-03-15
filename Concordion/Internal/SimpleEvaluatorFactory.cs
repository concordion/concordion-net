using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal
{
    public class SimpleEvaluatorFactory : IEvaluatorFactory
    {
        #region IEvaluatorFactory Members

        public IEvaluator CreateEvaluator(object fixture)
        {
            return new SimpleEvaluator(fixture);
        }

        #endregion
    }
}
