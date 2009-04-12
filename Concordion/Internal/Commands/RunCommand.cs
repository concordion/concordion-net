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
using Concordion.Internal.Util;

namespace Concordion.Internal.Commands
{
    public class RunCommand : ICommand
    {
        #region ICommand Members

        public void SetUp(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
        }

        public void Execute(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            Check.IsFalse(commandCall.HasChildCommands, "Nesting commands inside a 'run' is not supported");

            Element element = commandCall.Element;

            String href = element.GetAttributeValue("href");
            Check.NotNull(href, "The 'href' attribute must be set for an element containing concordion:run");

            String runnerType = commandCall.Expression;
            String expression = element.GetAttributeValue("concordion:params");

            if (expression != null)
            {
                evaluator.Evaluate(expression);
            }

            String concordionRunner = null;

            // TODO - determine the appropriate concordion runner here and use it
            //concordionRunner = System.getProperty("concordion.runner." + runnerType);

            //if (concordionRunner == null && "concordion".equals(runnerType)) 
            //{
            //    concordionRunner = DefaultConcordionRunner.class.getName();
            //}

            //if (concordionRunner == null) 
            //{
            //    try 
            //    {
            //        Class.forName(runnerType);
            //        concordionRunner = runnerType;
            //    } 
            //    catch (ClassNotFoundException e1) 
            //    {
            //        // OK, we're reporting this in a second.
            //    }
            //}

            Check.NotNull(concordionRunner, "The runner '" + runnerType + "' cannot be found. "
                    + "Choices: (1) Use 'concordion' as your runner (2) Ensure that the 'concordion.runner." + runnerType
                    + "' System property is set to a name of an org.concordion.Runner implementation "
                    + "(3) Specify a full class name of an org.concordion.Runner implementation");
            //try 
            //{
            //    Class<?> clazz = Class.forName(concordionRunner);
            //    Runner runner = (Runner) clazz.newInstance();
            //    for (Method method : runner.getClass().getMethods()) {
            //        String methodName = method.getName();
            //        if (methodName.startsWith("set") && methodName.length() > 3 && method.getParameterTypes().length == 1) {
            //            String variableName = methodName.substring(3, 4).toLowerCase() + method.getName().substring(4);
            //            Object variableValue = evaluator.evaluate(variableName);
            //            if (variableValue == null) {
            //                try {
            //                    variableValue = evaluator.getVariable(variableName);
            //                } catch (Exception e) {
            //                }
            //            }
            //            if (variableValue != null) {
            //                try {
            //                    method.invoke(runner, variableValue);
            //                } catch (Exception e) {
            //                }
            //            }
            //        }
            //    }
            //    try {
            //        Result result = runner.execute(commandCall.getResource(), href).getResult();

            //        if (result == Result.SUCCESS) {
            //            announceSuccess(element);
            //        } else if (result == Result.IGNORED) {
            //            announceIgnored(element);
            //        } else {
            //            announceFailure(element);
            //        }
            //        resultRecorder.record(result);
            //    } catch (Throwable e) {
            //        announceFailure(e, element, runnerType);
            //        resultRecorder.record(Result.FAILURE);
            //    }
            //} catch (Exception e) {
            //    announceFailure(e, element, runnerType);
            //    resultRecorder.record(Result.FAILURE);
            //}
        }

        public void Verify(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
        }

        #endregion
    }
}
