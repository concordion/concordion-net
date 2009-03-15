using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Concordion.Api;

namespace Concordion.Internal.Commands
{
    public abstract class AbstractCommandDecorator : ICommand
    {
        #region Fields
        
        protected readonly ICommand m_command; 

        #endregion

        #region Constructors

        public AbstractCommandDecorator(ICommand command)
        {
            m_command = command;
        }

        #endregion

        #region Methods

        //protected abstract void Process(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder, Thread runner);

        #endregion

        #region ICommand Members

        public abstract void SetUp(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder);
        public abstract void Execute(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder);
        public abstract void Verify(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder);

        #endregion
    }
}
