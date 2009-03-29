using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Internal.Renderer;
using System.Xml.Linq;
using Concordion.Spec.Support;
using System.Diagnostics;
using Concordion.Internal.Commands;
using Concordion.Api;

namespace Concordion.Spec.Concordion.Results.Exception
{
    class ExceptionTest
    {
        private List<string> stackTraceElements = new List<string>();

        public void addStackTraceElement(string declaringClassName, string methodName, string filename, int lineNumber)
        {
            stackTraceElements.Add(String.Format("at {0}.{1} in {2}:line {3}", declaringClassName, methodName, filename, lineNumber));
        }

        public string markAsException(string fragment, string expression, string errorMessage)
        {
            var exception = new StackTraceSettingException(errorMessage);
            exception.StackTraceElements.AddRange(stackTraceElements);

            var document = new TestRig()
                                .ProcessFragment(fragment)
                                .GetXDocument();

            var element = document.Descendants("p").ToArray()[0];

            var eventArgs = new ExceptionCaughtEventArgs { Exception = exception, Expression = expression, Element = new Element(element) };
            new ExceptionRenderer().ExceptionCaughtEventHandler(this, eventArgs);

            return element.ToString(SaveOptions.DisableFormatting);
        }
    }
}
