using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Internal.Util;

namespace Concordion.Internal.Commands
{
    public class EchoCommand : ICommand
    {
        #region ICommand Members

        public void SetUp(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
        }

        public void Execute(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
        }

        public void Verify(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            Check.IsFalse(commandCall.HasChildCommands, "Nesting commands inside an 'echo' is not supported");

            Object result = evaluator.Evaluate(commandCall.Expression);

            Element element = commandCall.Element;
            if (result != null)
            {
                element.AppendText(result.ToString());
            }
            else
            {
                Element child = new Element("em");
                child.AppendText("null");
                element.AppendChild(child);
            }
        }

        #endregion
    }
}
