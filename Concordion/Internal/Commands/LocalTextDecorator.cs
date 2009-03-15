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

        public override void SetUp(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            object savedTextValue = evaluator.GetVariable(TEXT_VARIABLE);
            object savedHrefValue = evaluator.GetVariable(HREF_VARIABLE);
            try
            {
                evaluator.SetVariable(TEXT_VARIABLE, commandCall.Element.Text);
                evaluator.SetVariable(HREF_VARIABLE, getHref(commandCall.Element));
                m_command.SetUp(commandCall, evaluator, resultRecorder);
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
