using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Internal.Commands
{
    internal class TableExecuteStrategy : IExecuteStrategy
    {
        #region IExecuteStrategy Members

        public void Execute(CommandCall commandCall, global::Concordion.Api.IEvaluator evaluator, global::Concordion.Api.IResultRecorder resultRecorder)
        {
            TableSupport tableSupport = new TableSupport(commandCall);
            IList<Row> detailRows = tableSupport.GetDetailRows();
            foreach (Row detailRow in detailRows) 
            {
                if (detailRow.GetCells().Count != tableSupport.ColumnCount) 
                {
                    throw new Exception("The <table> 'execute' command only supports rows with an equal number of columns.");
                }

                commandCall.Element = detailRow.RowElement;
                tableSupport.CopyCommandCallsTo(detailRow);
                commandCall.Execute(evaluator, resultRecorder);
            }
        }

        #endregion
    }
}
