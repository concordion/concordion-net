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
using System.Threading;

namespace Concordion.Internal.Commands
{
    public class LocalTextDecorator : AbstractCommandDecorator
    {
        private static readonly string TEXT_VARIABLE = "#TEXT";
        private static readonly string HREF_VARIABLE = "#HREF";

        public LocalTextDecorator(ICommand command)
            : base(command)
        {
        }

        public override void Setup(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            object savedTextValue = evaluator.GetVariable(TEXT_VARIABLE);
            object savedHrefValue = evaluator.GetVariable(HREF_VARIABLE);
            try
            {
                evaluator.SetVariable(TEXT_VARIABLE, commandCall.Element.Text);
                evaluator.SetVariable(HREF_VARIABLE, getHref(commandCall.Element));
                m_command.Setup(commandCall, evaluator, resultRecorder);
            }
            finally
            {
                evaluator.SetVariable(TEXT_VARIABLE, savedTextValue);
                evaluator.SetVariable(HREF_VARIABLE, savedHrefValue);
            }
        }

        public override void Execute(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            object savedTextValue = evaluator.GetVariable(TEXT_VARIABLE);
            object savedHrefValue = evaluator.GetVariable(HREF_VARIABLE);
            try
            {
                evaluator.SetVariable(TEXT_VARIABLE, commandCall.Element.Text);
                evaluator.SetVariable(HREF_VARIABLE, getHref(commandCall.Element));
                m_command.Execute(commandCall, evaluator, resultRecorder);
            }
            finally
            {
                evaluator.SetVariable(TEXT_VARIABLE, savedTextValue);
                evaluator.SetVariable(HREF_VARIABLE, savedHrefValue);
            }
        }

        public override void Verify(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            object savedTextValue = evaluator.GetVariable(TEXT_VARIABLE);
            object savedHrefValue = evaluator.GetVariable(HREF_VARIABLE);
            try
            {
                evaluator.SetVariable(TEXT_VARIABLE, commandCall.Element.Text);
                evaluator.SetVariable(HREF_VARIABLE, getHref(commandCall.Element));
                m_command.Verify(commandCall, evaluator, resultRecorder);
            }
            finally
            {
                evaluator.SetVariable(TEXT_VARIABLE, savedTextValue);
                evaluator.SetVariable(HREF_VARIABLE, savedHrefValue);
            }
        }

        private string getHref(Element element)
        {
            string href = element.GetAttributeValue("href");
            if (href == null)
            {
                Element a = element.GetFirstChildElement("a");
                if (a != null)
                {
                    href = a.GetAttributeValue("href");
                }
            }
            return href;
        }


    }
}
