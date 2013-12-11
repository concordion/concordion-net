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
            if (evaluator.GetVariable(LEVEL_VARIABLE) == null)
            {
                evaluator.SetVariable(LEVEL_VARIABLE, 0);
            }
            ListSupport listSupport = new ListSupport(commandCall);
            IList<ListEntry> listEntries = listSupport.GetListEntries();
            foreach (var listEntry in listEntries)
            {
                if (listEntry.IsItem)
                {
                    commandCall.Element = listEntry.Element;
                    commandCall.Execute(evaluator, resultRecorder);
                }
                if (listEntry.IsList)
                {
                    int level = (int) evaluator.GetVariable(LEVEL_VARIABLE);
                    evaluator.SetVariable(LEVEL_VARIABLE, ++level);
                    commandCall.Element = listEntry.Element;
                    Execute(commandCall, evaluator, resultRecorder);
                }
            }


            throw new NotImplementedException();
        }
    }
}
