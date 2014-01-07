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

namespace Concordion.Internal.Commands
{
    internal class DefaultExecuteStrategy : IExecuteStrategy
    {
        private readonly ExecuteCommand m_ExecuteCommand;

        public DefaultExecuteStrategy(ExecuteCommand executeCommand)
        {
            this.m_ExecuteCommand = executeCommand;
        }

        #region IExecuteStrategy Members

        public void Execute(CommandCall commandCall, global::Concordion.Api.IEvaluator evaluator, global::Concordion.Api.IResultRecorder resultRecorder)
        {
            CommandCallList childCommands = commandCall.Children;

            childCommands.SetUp(evaluator, resultRecorder);
            evaluator.Evaluate(commandCall.Expression);
            childCommands.Execute(evaluator, resultRecorder);
            m_ExecuteCommand.AnnounceExecuteCompleted(commandCall.Element);
            childCommands.Verify(evaluator, resultRecorder);
        }

        #endregion
    }
}
