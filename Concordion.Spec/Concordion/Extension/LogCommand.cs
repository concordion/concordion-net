using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Internal;

namespace Concordion.Spec.Concordion.Extension
{
    public class LogCommand : ICommand
    {
        private TextWriter LogWriter { get; set; }

        public LogCommand(TextWriter logWriter)
        {
            this.LogWriter = logWriter;
        }

        public void Setup(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
        }

        public void Execute(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            LogWriter.WriteLine(commandCall.Element.Text);
        }

        public void Verify(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
        }
    }
}
