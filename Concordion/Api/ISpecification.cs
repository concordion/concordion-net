using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api
{
    public interface ISpecification
    {
        void Process(IEvaluator evaluator, IResultRecorder resultRecorder);
    }
}
