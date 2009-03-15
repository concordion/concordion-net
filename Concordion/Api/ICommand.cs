using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Internal;

namespace Concordion.Api
{
    public interface ICommand
    {
        void SetUp(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder);
        void Execute(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder);
        void Verify(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder);
    }
}
