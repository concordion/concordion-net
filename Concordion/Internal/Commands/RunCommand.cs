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
using Concordion.Api.Listener;
using Concordion.Internal.Util;
using Concordion.Internal.Runner;
using System.Reflection;

namespace Concordion.Internal.Commands
{
    public class RunCommand : AbstractCommand
    {
        private List<IRunListener> m_Listeners = new List<IRunListener>();

        #region Properties

        public Dictionary<string, IRunner> Runners
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public void AddRunListener(IRunListener runListener)
        {
            m_Listeners.Add(runListener);
        }

        public void RemoveRunListener(IRunListener runListener)
        {
            m_Listeners.Remove(runListener);
        }

        private void AnnounceIgnored(Element element)
        {
            foreach (var listener in m_Listeners)
            {
                listener.IgnoredReported(new RunIgnoreEvent(element));
            }
        }

        private void AnnounceSuccess(Element element)
        {
            foreach (var listener in m_Listeners)
            {
                listener.SuccessReported(new RunSuccessEvent(element));
            }
        }

        private void AnnounceFailure(Element element)
        {
            foreach (var listener in m_Listeners)
            {
                listener.FailureReported(new RunFailureEvent(element));
            }
        }

        private void AnnounceFailure(Exception exception, Element element, string expression)
        {
            foreach (var listener in m_Listeners)
            {
                listener.ExceptionCaught(new ExceptionCaughtEvent(exception, element, expression));
            }
        }

        #endregion

        #region Constructors

        public RunCommand()
        {
            Runners = new Dictionary<string, IRunner>();
        }

        #endregion

        #region ICommand Members

        public override void Execute(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            Check.IsFalse(commandCall.HasChildCommands, "Nesting commands inside a 'run' is not supported");

            var element = commandCall.Element;

            var href = element.GetAttributeValue("href");
            Check.NotNull(href, "The 'href' attribute must be set for an element containing concordion:run");

            var runnerType = commandCall.Expression;
            var expression = element.GetAttributeValue("params", "concordion");

            if (expression != null)
            {
                evaluator.Evaluate(expression);
            }

            try
            {
                IRunner concordionRunner;
                Runners.TryGetValue(runnerType, out concordionRunner);

                // TODO - re-check this.
                Check.NotNull(concordionRunner, "The runner '" + runnerType + "' cannot be found. "
                        + "Choices: (1) Use 'concordion' as your runner (2) Ensure that the 'concordion.runner." + runnerType
                        + "' System property is set to a name of an IRunner implementation "
                        + "(3) Specify an assembly fully qualified class name of an IRunner implementation");

                var result = concordionRunner.Execute(evaluator.Fixture, commandCall.Resource, href).Result;

                if (result == Result.Success)
                {
                    AnnounceSuccess(element);
                }
                else if (result == Result.Ignored)
                {
                    AnnounceIgnored(element);
                }
                else
                {
                    AnnounceFailure(element);
                }

                resultRecorder.Record(result);
            }
            catch
            {
                AnnounceFailure(element);
                resultRecorder.Record(Result.Failure);
            }
        }

        #endregion
    }
}
