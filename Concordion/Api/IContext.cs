using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api
{
    public interface IContext : IResultRecorder, IEvaluator
    {
        Resource Resource { get; }
        Element Element { get; }
        string Expression { get; }
        object EvaluateExpression();
        void ProcessChildCommandsSequentially();
        bool HasChildCommands();
        void SetUpChildCommands();
        void ExecuteChildCommands();
        void VerifyChildCommands();
    }
}
