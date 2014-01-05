using System.IO;
using Concordion.Api;
using Concordion.Internal;

namespace Concordion.Spec.Concordion.Extension.Command
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
            this.LogWriter.WriteLine(commandCall.Element.Text);
        }

        public void Verify(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
        }
    }
}
