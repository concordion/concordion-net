using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Internal.Util;

namespace Concordion.Internal.Commands
{
    public class AssertEqualsCommand : ICommand, IResultReporter
    {
        #region Fields

        private IComparer<object> m_comparer;

        #endregion

        #region Constructors

        public AssertEqualsCommand()
            : this(new BrowserStyleWhitespaceComparer())
        {
        }

        public AssertEqualsCommand(IComparer<object> comparer)
        {
            m_comparer = comparer;
        }

        #endregion

        #region Methods

        private void OnSuccessReported(Element element)
        {
            if (SuccessReported != null)
            {
                SuccessReported(this, new SuccessReportedEventArgs { Element = element });
            }
        }

        private void OnFailureReported(Element element, object actual, string expected)
        {
            if (FailureReported != null)
            {
                FailureReported(this, new FailureReportedEventArgs { Element = element, Actual = actual, Expected = expected });
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
            Check.IsFalse(commandCall.HasChildCommands, "Nesting commands inside an 'assertEquals' is not supported");

            Element element = commandCall.Element;

            object actual = evaluator.Evaluate(commandCall.Expression);
            string expected = element.Text;

            if (m_comparer.Compare(actual, expected) == 0)
            {
                resultRecorder.Record(Result.Success);
                OnSuccessReported(element);
            }
            else
            {
                resultRecorder.Record(Result.Failure);
                OnFailureReported(element, actual, expected);
            }
        }

        #endregion

        #region IResultReporter Members

        public event EventHandler<SuccessReportedEventArgs> SuccessReported;
        public event EventHandler<FailureReportedEventArgs> FailureReported;

        #endregion
    }
}
