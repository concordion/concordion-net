using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Concordion.Test.Support;
using NUnit.Framework;
using Concordion.Internal.Listener;

namespace Concordion.Test.Listener
{
    [TestFixture]
    public class MetadataCreatorTest
    {
        private MetadataCreator metadataCreator;
        private XElement html; 
        private XDocument document;
        private XElement head;
    
        [SetUp]
        public void Init() {
            html = new XElement("html");
            head = new XElement("head");
            html.Add(head);
            document = new XDocument(html);
            metadataCreator = new MetadataCreator();
        }
    
        [Test]
        public void AddsContentTypeMetadataIfMissing() {
            metadataCreator.BeforeParsing(document);
            Assert.AreEqual(
                "<html><head><meta http-equiv=\"content-type\" content=\"text/html; charset=UTF-8\" /></head></html>",
                new HtmlUtil().RemoveWhitespaceBetweenTags(html.ToString()));
        }

        [Test]
        public void DoesNotAddContentTypeMetadataIfAlreadyPresent() {
            var meta = new XElement("meta");
            meta.SetAttributeValue("http-equiv", "Content-Type");
            meta.SetAttributeValue("content", "text/html; charset=UTF-8");
            head.Add(new XElement(meta));
            Assert.AreEqual(
                "<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" /></head></html>",
                new HtmlUtil().RemoveWhitespaceBetweenTags(html.ToString()));
            metadataCreator.BeforeParsing(document);
            Assert.AreEqual(
                "<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" /></head></html>",
                new HtmlUtil().RemoveWhitespaceBetweenTags(html.ToString()));
        }
    }
}
