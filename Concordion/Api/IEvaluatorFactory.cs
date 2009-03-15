using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api
{
    public interface IEvaluatorFactory
    {
        IEvaluator CreateEvaluator(object fixture);
    }
}
