using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using System.Text.RegularExpressions;
using Concordion.Internal.Util;
using System.Collections;

namespace Concordion.Internal.Commands
{
    public class VerifyRowsCommand : ICommand
    {
        #region Methods

        private void OnSurplusRow(Element element)
        {
            if (SurplusRowFound != null)
            {
                SurplusRowFound(this, new SurplusRowEventArgs { RowElement = element });
            }
        }

        private void OnMissingRow(Element element)
        {
            if (MissingRowFound != null)
            {
                MissingRowFound(this, new MissingRowEventArgs { RowElement = element });
            }
        }

        #endregion

        #region ICommand Members

        public void SetUp(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
        }

        public void Execute(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
        }

        public void Verify(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            Regex pattern = new Regex("(#.+?) *: *(.+)");
            Match matcher = pattern.Match(commandCall.Expression);
            if (!matcher.Success) 
            {
                throw new InvalidOperationException("The expression for a \"verifyRows\" should be of the form: #var : collectionExpr");
            }

            string loopVariableName = matcher.Groups[1].Value;
            string iterableExpression = matcher.Groups[2].Value;

            object obj = evaluator.Evaluate(iterableExpression);

            Check.NotNull(obj, "Expression returned null (should be an IEnumerable).");
            Check.IsTrue(obj is IEnumerable, obj.GetType() + " is not IEnumerable");
            Check.IsTrue(!(obj is IDictionary), obj.GetType() + " does not have a predictable iteration order");

            IEnumerable iterable = (IEnumerable) obj;
            
            TableSupport tableSupport = new TableSupport(commandCall);
            IList<Row> detailRows = tableSupport.GetDetailRows();

            int index = 0;
            foreach (object loopVar in iterable) 
            {
                evaluator.SetVariable(loopVariableName, loopVar);
                Row detailRow;
                if (detailRows.Count > index) 
                {
                    detailRow = detailRows[index];
                } 
                else 
                {
                    detailRow = tableSupport.AddDetailRow();
                    OnSurplusRow(detailRow.RowElement);
                }
                tableSupport.CopyCommandCallsTo(detailRow);
                commandCall.Children.Verify(evaluator, resultRecorder);
                index++;
            }
            
            for (; index < detailRows.Count; index++) {
                Row detailRow = detailRows[index];
                resultRecorder.Record(Result.Failure);
                OnMissingRow(detailRow.RowElement);
            }
        }

        #endregion

        #region Events

        public event EventHandler<SurplusRowEventArgs> SurplusRowFound;
        public event EventHandler<MissingRowEventArgs> MissingRowFound;

        #endregion
    }
}
