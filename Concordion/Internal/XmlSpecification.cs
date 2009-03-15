using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal
{
    public class XmlSpecification : ISpecification
    {
        private CommandCall RootCommandNode
        {
            get;
            set;
        }

        public XmlSpecification(CommandCall rootCommandNode)
        {
            RootCommandNode = rootCommandNode;
        }

        public void Process(IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            RootCommandNode.Execute(evaluator, resultRecorder);
        }
    }
}
