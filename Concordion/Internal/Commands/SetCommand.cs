using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Internal.Util;

namespace Concordion.Internal.Commands
{
    public class SetCommand : ICommand
    {
        #region ICommand Members

        public void SetUp(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            Check.IsFalse(commandCall.HasChildCommands, "Nesting commands inside a 'set' is not supported");
            evaluator.SetVariable(commandCall.Expression, commandCall.Element.Text);
        }

        public void Execute(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
        }

        public void Verify(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
        }

        #endregion
    }
}
