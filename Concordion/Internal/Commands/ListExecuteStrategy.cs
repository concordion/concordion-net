using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal.Commands
{
    class ListExecuteStrategy : IExecuteStrategy
    {
        private static readonly string LEVEL_VARIABLE = "#LEVEL";

        public void Execute(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            increaseLevel(evaluator);
            ListSupport listSupport = new ListSupport(commandCall);
            foreach (Element listItem in listSupport.GetListItemElements())
            {
                commandCall.Element = listItem;
                commandCall.Execute(evaluator, resultRecorder);
            }
            foreach (Element listElement in listSupport.GetListElements())
            {
                commandCall.Element = listElement;
                Execute(commandCall, evaluator, resultRecorder);
            }
            decreaseLevel(evaluator);
        }

        private void increaseLevel(IEvaluator evaluator)
        {
            if (evaluator.GetVariable(LEVEL_VARIABLE) == null)
            {
                evaluator.SetVariable(LEVEL_VARIABLE, 1);
            }
            else
            {
                int level = (int)evaluator.GetVariable(LEVEL_VARIABLE);
                level = level++;
                evaluator.SetVariable(LEVEL_VARIABLE, level);
            }
        }

        private void decreaseLevel(IEvaluator evaluator)
        {
            int level = (int)evaluator.GetVariable(LEVEL_VARIABLE);
            level = level--;
            evaluator.SetVariable(LEVEL_VARIABLE, level);
        }
    }
}
