using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Integration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Concordion.Spec
{
    [TestClass]
    [HtmlResource("Demo")]
    public class DemoTest : ConcordionTestCase
    {
        public string GreetingFor(string firstName)
        {
            return string.Format("Hello {0}!", firstName);
        }
    }
}
