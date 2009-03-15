using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal
{
    public class CommandCall
    {

        #region Properties
        
        public CommandCallList Children
        {
            get;
            private set;
        }

        public ICommand Command
        {
            get;
            private set;
        }

        public Resource Resource
        {
            get;
            private set;
        }

        public Element Element
        {
            get;
            set;
        }

        public string Expression
        {
            get;
            private set;
        }

        public bool HasChildCommands
        {
            get
            {
                return !Children.IsEmpty;
            }
        }

        #endregion


        public CommandCall(ICommand command, Element element, string expression, Resource resource) 
        {
            Children = new CommandCallList();
            Command = command;
            Element = element;
            Expression = expression;
            Resource = resource;
        }

        #region Methods

        public void SetUp(IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            Command.SetUp(this, evaluator, resultRecorder);
        }

        public void Execute(IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            Command.Execute(this, evaluator, resultRecorder);
        }

        public void Verify(IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            Command.Verify(this, evaluator, resultRecorder);
        }

        public void AddChild(CommandCall child)
        {
            Children.Add(child);
        }

        #endregion
    }
}
