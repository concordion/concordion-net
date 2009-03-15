using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal.Commands
{
    internal interface IExecuteStrategy
    {
        void Execute(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder);
    }
}
