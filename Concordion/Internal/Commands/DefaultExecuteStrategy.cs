using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Internal.Commands
{
    internal class DefaultExecuteStrategy : IExecuteStrategy
    {
        #region IExecuteStrategy Members

        public void Execute(CommandCall commandCall, global::Concordion.Api.IEvaluator evaluator, global::Concordion.Api.IResultRecorder resultRecorder)
        {
            CommandCallList childCommands = commandCall.Children;

            childCommands.SetUp(evaluator, resultRecorder);
            evaluator.Evaluate(commandCall.Expression);
            childCommands.Execute(evaluator, resultRecorder);
            childCommands.Verify(evaluator, resultRecorder);
        }

        #endregion
    }
}
