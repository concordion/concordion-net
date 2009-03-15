using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal.Commands
{
    public class ExecuteCommand : ICommand
    {
        #region ICommand Members

        public void SetUp(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
        }

        public void Execute(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            IExecuteStrategy strategy;
            if (commandCall.Element.IsNamed("table"))
            {
                strategy = new TableExecuteStrategy();
            }
            else
            {
                strategy = new DefaultExecuteStrategy();
            }
            strategy.Execute(commandCall, evaluator, resultRecorder);
        }

        public void Verify(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
        }

        #endregion
    }
}
