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
using Concordion.Api;
using Concordion.Api.Listener;
using Concordion.Internal.Util;

namespace Concordion.Internal.Listener
{
    public class ExceptionRenderer : IExceptionCaughtListener
    {
        #region Fields

        private long buttonId = 0;
        
        #endregion

        #region Properties

        private ICollection<Element> RootElementsWithScript
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public ExceptionRenderer()
        {
            this.RootElementsWithScript = new HashSet<Element>();
        }

        #endregion

        #region Methods

        private Element ExpectedSpan(Element element)
        {
            Element spanExpected = new Element("del").AddStyleClass("expected");
            element.MoveChildrenTo(spanExpected);
            spanExpected.AppendNonBreakingSpaceIfBlank();
            Element spanFailure = new Element("span").AddStyleClass("failure");
            spanFailure.AppendChild(spanExpected);
            return spanFailure;
        }

        private Element ExceptionMessage(string exceptionMessage)
        {
            return new Element("span")
                    .AddStyleClass("exceptionMessage")
                    .AppendText(exceptionMessage);
        }

        private Element StackTraceTogglingButton()
        {
            return new Element("input")
                    .AddStyleClass("stackTraceButton")
                    .SetId("stackTraceButton" + this.buttonId)
                    .AddAttribute("type", "button")
                    .AddAttribute("onclick", "javascript:toggleStackTrace('" + this.buttonId + "')")
                    .AddAttribute("value", "View Stack");
        }

        private Element StackTrace(Exception exception, string expression)
        {
            Element stackTrace = new Element("span").AddStyleClass("stackTrace");
            stackTrace.SetId("stackTrace" + this.buttonId);

            Element p = new Element("p")
                    .AppendText("While evaluating expression: ");
            p.AppendChild(new Element("code").AppendText(expression));
            stackTrace.AppendChild(p);

            var stackTraceElements = new List<string> { String.Format("{0}: {1}", exception.GetType().ToString(), exception.Message) };
            stackTraceElements.AddRange(exception.StackTrace.ToString().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));

            foreach (var stackTraceElement in stackTraceElements)
            {
                stackTrace.AppendChild(this.StackTraceElement(stackTraceElement));
            }

            //RecursivelyAppendStackTrace(exception, stackTrace);

            return stackTrace;
        }

        private void RecursivelyAppendStackTrace(Exception exception, Element stackTrace)
        {
            Element stackTraceExceptionMessage = new Element("span")
                    .AddStyleClass("stackTraceExceptionMessage")
                    .AppendText(exception.GetType().Name + ": " + exception.Message);
            stackTrace.AppendChild(stackTraceExceptionMessage);
            stackTrace.AppendChild(this.StackTraceElement(exception.StackTrace));

            // TODO - Figure out if this is needed any longer
            //if (exception is OgnlException) 
            //{
            //    Exception reason = ((OgnlException) exception).getReason();
            //    if (reason != null) 
            //    {
            //        RecursivelyAppendStackTrace(reason, stackTrace);
            //    }
            //}

            if (exception.InnerException != null) 
            {
                this.RecursivelyAppendStackTrace(exception.InnerException, stackTrace);
            }
        }

        private Element StackTraceElement(string stackTraceText)
        {
            Element entry = new Element("span")
                    .AddStyleClass("stackTraceEntry")
                    .AppendText(stackTraceText);
            return entry;
        }

        private void EnsureDocumentHasTogglingScript(Element element)
        {
            Element rootElement = element.GetRootElement();

            if (!this.RootElementsWithScript.Contains(rootElement))
            {
                this.RootElementsWithScript.Add(rootElement);
                Element head = rootElement.GetFirstDescendantNamed("head");
                if (head == null)
                {
                    Console.WriteLine(rootElement.ToXml());
                }
                Check.NotNull(head, "Document <head> section is missing");
                Element script = new Element("script").AddAttribute("type", "text/javascript");
                head.PrependChild(script);
                script.AppendText(HtmlFramework.TOGGLING_SCRIPT_RESOURCE);
            }
        }

        #endregion

        #region IExceptionCaughtListener Members

        public void ExceptionCaught(ExceptionCaughtEvent caughtEvent)
        {
            this.buttonId++;
            var element = caughtEvent.Element;
            element.AppendChild(this.ExpectedSpan(element));
            // Special handling for <a> tags to avoid the stack-trace being inside the link text
            if (element.IsNamed("a"))
            {
                var div = new Element("div");
                element.AppendSister(div);
                element = div;
            }
            element.AppendChild(this.ExceptionMessage(caughtEvent.CaughtException.Message));
            element.AppendChild(this.StackTraceTogglingButton());
            element.AppendChild(this.StackTrace(caughtEvent.CaughtException, caughtEvent.Expression));

            this.EnsureDocumentHasTogglingScript(element);
        }

        #endregion

    }
}
