using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Spec.Concordion.Results.Exception
{
    class ExceptionTest
    {
        // TODO - repair because the implementation of exceptions differs from .Net to Java
        //private List<StackTraceElement> stackTraceElements = new List<StackTraceElement>();

        //public void addStackTraceElement(String declaringClassName, String methodName, String filename, int lineNumber)
        //{
        //    if (filename.equals("null"))
        //    {
        //        filename = null;
        //    }
        //    stackTraceElements.add(new StackTraceElement(declaringClassName, methodName, filename, lineNumber));
        //}

        //public String markAsException(String fragment, String expression, String errorMessage)
        //{
        //    Throwable t = new Throwable(errorMessage);
        //    t.setStackTrace(stackTraceElements.toArray(new StackTraceElement[0]));

        //    Element element = new Element((nu.xom.Element)new TestRig()
        //        .processFragment(fragment)
        //        .getXOMDocument()
        //        .query("//p")
        //        .get(0));

        //    new ThrowableRenderer().throwableCaught(new ThrowableCaughtEvent(t, element, expression));

        //    return element.toXML();
        //}
    }
}
