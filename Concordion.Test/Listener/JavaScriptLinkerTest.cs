using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Concordion.Api;
using NUnit.Framework;
using Concordion.Internal.Listener;

namespace Concordion.Test.Listener
{
    [TestFixture]
    public class JavaScriptLinkerTest
    {
        private static readonly Resource NOT_NEEDED_PARAMETER = null;

        [Test]
        public void XmlOutputContainsAnExplicitEndTagForScriptElement()
        {
            var javaScriptLinker = new JavaScriptLinker(NOT_NEEDED_PARAMETER);

            var html = new XElement("html");
            var head = new XElement("head");
            html.Add(head);

            javaScriptLinker.BeforeParsing(new XDocument(html));

            var expected = "<head><script type=\"text/javascript\"></script></head>";
            var actual = head.ToString().Replace("\n", "").Replace("\r", "").Replace("> ", ">").Replace(" <", "<");
            Assert.AreEqual(expected, actual);
        }
    }
}
