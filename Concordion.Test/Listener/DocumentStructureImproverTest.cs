using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Concordion.Internal.Listener;
using Concordion.Test.Support;
using NUnit.Framework;

namespace Concordion.Test.Listener
{
    [TestFixture]
    public class DocumentStructureImproverTest
    {
        private DocumentStructureImprover improver;
        private XElement html;
        private XDocument document;

        [SetUp]
        public void Init()
        {
            improver = new DocumentStructureImprover();
            html = new XElement("html");
            document = new XDocument(html);
        }
    
        [Test]
        public void AddsHeadIfMissing()
        {
            improver.BeforeParsing(document);
            Assert.AreEqual(
                "<html><head /></html>", 
                new HtmlUtil().RemoveWhitespaceBetweenTags(html.ToString()));

            // Check it does not add it again if we repeat the call
            improver.BeforeParsing(document);
            Assert.AreEqual(
                "<html><head /></html>", 
                new HtmlUtil().RemoveWhitespaceBetweenTags(html.ToString()));
        }

        [Test]
        public void TransfersEverythingBeforeBodyIntoNewlyCreatedHead()
        {
            var style1 = new XElement("style1");
            var style2 = new XElement("style2");
            html.Add(style1);
            html.Add(style2);

            var body = new XElement("body");
            body.SetValue("some ");
            var bold = new XElement("b");
            bold.SetValue("bold text");
            body.Add(bold);
            html.Add(body);
            improver.BeforeParsing(document);

            Assert.AreEqual(
                "<html><head><style1 /><style2 /></head><body>some <b>bold text</b></body></html>", 
                new HtmlUtil().RemoveWhitespaceBetweenTags(html.ToString()));
        }
    }
}
