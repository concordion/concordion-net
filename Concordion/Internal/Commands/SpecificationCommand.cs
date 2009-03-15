using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal.Commands
{
    public class SpecificationCommand : ICommand
    {
        #region Methods

        private void OnSpecificationCommandProcessing(Resource resource, Element element)
        {
            if (SpecificationCommandProcessing != null)
            {
                SpecificationCommandProcessing(this, new SpecificationEventArgs { Element = element, Resource = resource });
            }
        }

        private void OnSpecificationCommandProcessed(Resource resource, Element element)
        {
            if (SpecificationCommandProcessed != null)
            {
                SpecificationCommandProcessed(this, new SpecificationEventArgs { Element = element, Resource = resource });
            }
        }

        #endregion

        #region ICommand Members

        public void SetUp(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            throw new InvalidOperationException("Unexpected call to SpecificationCommand's SetUp() method. Only the Execute() method should be called.");
        }

        public void Execute(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            OnSpecificationCommandProcessing(commandCall.Resource, commandCall.Element);
            commandCall.Children.ProcessSequentially(evaluator, resultRecorder);
            OnSpecificationCommandProcessed(commandCall.Resource, commandCall.Element);
        }

        public void Verify(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            throw new InvalidOperationException("Unexpected call to SpecificationCommand's Verify() method. Only the Execute() method should be called.");
        }

        #endregion

        #region Events

        public event EventHandler<SpecificationEventArgs> SpecificationCommandProcessing;
        public event EventHandler<SpecificationEventArgs> SpecificationCommandProcessed;

        #endregion
    }
}
