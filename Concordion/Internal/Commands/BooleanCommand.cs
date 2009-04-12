// Copyright 2009 Jeffrey Cameron
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using System.Data;

namespace Concordion.Internal.Commands
{
    public abstract class BooleanCommand : ICommand, IResultReporter
    {
        #region Methods

        protected void OnSuccessReported(Element element)
        {
            if (SuccessReported != null)
            {
                SuccessReported(this, new SuccessReportedEventArgs { Element = element });
            }
        }

        protected void OnFailureReported(Element element, object actual, string expected)
        {
            if (FailureReported != null)
            {
                FailureReported(this, new FailureReportedEventArgs { Element = element, Actual = actual, Expected = expected });
            }
        }

        protected abstract void ProcessFalseResult(CommandCall commandCall,IResultRecorder resultRecorder);
        protected abstract void ProcessTrueResult(CommandCall commandCall, IResultRecorder resultRecorder);

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
            CommandCallList childCommands = commandCall.Children;
            childCommands.SetUp(evaluator, resultRecorder);
            childCommands.Execute(evaluator, resultRecorder);
            childCommands.Verify(evaluator, resultRecorder);

            String expression = commandCall.Expression;
            Object result = evaluator.Evaluate(expression);

            if (result != null && result is Boolean) 
            {
                if ((Boolean) result) 
                {
            	    ProcessTrueResult(commandCall, resultRecorder);
                } 
                else 
                {
            	    ProcessFalseResult(commandCall, resultRecorder);
                }
            } 
            else 
            {
                throw new InvalidExpressionException("Expression '" + expression + "' did not produce a boolean result (needed for assertTrue).");
            }
        }

        #endregion

        #region IResultReporter Members

        public event EventHandler<SuccessReportedEventArgs> SuccessReported;
        public event EventHandler<FailureReportedEventArgs> FailureReported;

        #endregion
    }
}
