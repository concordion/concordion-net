using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal.Commands
{
    public abstract class AbstractCommand : ICommand
    {
        #region ICommand Members

        public abstract void SetUp(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder);
        public abstract void Execute(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder);
        public abstract void Verify(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder);

        #endregion
    }
}
