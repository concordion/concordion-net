using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api.Listener;
using Concordion.Internal.Listener;
using System.Xml.Linq;
using Concordion.Spec.Support;
using System.Diagnostics;
using Concordion.Internal.Commands;
using Concordion.Api;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Results.Exception
{
    [ConcordionTest]
    public class ExceptionTest
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

            var element = new Element(document.Descendants("p").ToArray()[0]);

            new ExceptionRenderer().ExceptionCaught(new ExceptionCaughtEvent(exception, element, expression));

            return element.ToXml();
        }
    }
}
