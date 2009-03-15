using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Integration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Concordion.Api;
using Concordion.Internal;

namespace Concordion.Spec
{
    [TestClass]
    [HtmlResource("Demo", @"C:\Dev\Concordion\Concordion.Spec.MsTest\Resources\Demo.html")]
    public class DemoTest : ConcordionTestCase
    {
        [TestMethod]
        public void RunTest()
        {
        }

        public string GreetingFor(string firstName)
        {
            return string.Format("Hello {0}!", firstName);
        }
    }
}
