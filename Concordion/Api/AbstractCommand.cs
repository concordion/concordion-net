using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Internal;

namespace Concordion.Api
{
    public abstract class AbstractCommand : ICommand
    {
        virtual public void Setup(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
        }

        virtual public void Execute(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
        }

        virtual public void Verify(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
        }
    }
}
