using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api
{
    public interface IEvaluator
    {
        object GetVariable(string variableName);
        void SetVariable(string variableName, object value);
        object Evaluate(string expression);
    }
}
